using Cars.BLL.ViewModels;
using Cars.BLL.DTOs.Auth;
using Cars.COMMON.Responses;
using Cars.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using Cars.COMMON.Constants;
using System.Security.Cryptography;
using Cars.COMMON.DTOs;
using Cars.DAL.DbContext;
using Microsoft.EntityFrameworkCore;
using Cars.BLL.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;

namespace Cars.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly CarDbContext _context;
        private readonly ILogger<AuthService> _logger;
        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            CarDbContext context,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseWithPayload<UserDTO>> Auth(LoginVm model)
        {
            ResponseWithPayload<UserDTO> response = new(new UserDTO());

            var user = await GetUserDTO(model);

            if (user == null)
            {
                return GetFailedResponse(response);
            }

            var userCheck = await _userManager.FindByEmailAsync(model.Email);

            if (userCheck.BannedTime != null)
            {
                response.Succeeded = false;
                response.Message = "The account is banned";
                return response;
            }

            if (userCheck.IsDeleted)
            {
                response.Succeeded = false;
                response.Message = "The account has been deleted";
                return response;
            }

            response.Data = user;
            response.Data.RefreshToken = await GenerateRefreshToken(model.Email);
            response.Data.JWTToken = GenerateJWT(user);

            return response;
        }

        private static T GetFailedResponse<T>(T response) where T : BaseResponse
        {
            response.Succeeded = false;
            response.Message = "Invalid email and password";
            return response;
        }
        public async Task<BaseResponse> ChangePassword(ChangePasswordVm model)
        {
            BaseResponse response = new();

            var user = await GetAuthenticatedUser(model);

            if (user == null)
            {
                response.Succeeded = false;
                response.Message = "Invalid email and password";
                return response;
            }

            var passwordChangeToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, passwordChangeToken, model.NewPassword);

            if (!result.Succeeded)
            {
                response.Succeeded = false;

                string message = $"Password change failed for the user {model.Email} " + (Environment.NewLine) + CollectErrors(result);
                _logger.LogError(message);
                response.Message = message;
            }

            return response;
        }

        public async Task<BaseResponse> ResetPassword(ForgotPasswordVm model)
        {
            BaseResponse response = new();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                response.Succeeded = false;
                response.Message = "Invalid email";
                return response;
            }
            var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await _userManager.ResetPasswordAsync(user, decoded, model.Password);

            if (!result.Succeeded)
            {
                string message = $"Password change failed for the user {model.Email} " + (Environment.NewLine) + CollectErrors(result);
                _logger.LogError(message);
                response.Succeeded = false;
                response.Message = message;
            }

            return response;
        }

        public async Task<ResponseWithPayload<ForgotPasswordVm>> ForgotPassword(string email)
        {
            ResponseWithPayload<ForgotPasswordVm> response = new(new ForgotPasswordVm());

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                response.Succeeded = false;
                response.Message = "Invalid email";
                return response;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            response.Data.Code = code;

            return response;
        }

        private async Task<ApplicationUser> GetAuthenticatedUser(ChangePasswordVm model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                return user;
            }

            return null;
        }

        private async Task<UserDTO> GetUserDTO(LoginVm model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return await MapUserToDTO(user);
            }

            return null;
        }

        private async Task<UserDTO> MapUserToDTO(ApplicationUser user)
        {
            var userWithRoles = await _context.Users
                        .Include(u => u.Roles)
                        .ThenInclude(ur => ur.Role)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == user.Id);

            return Mapper.Map<UserDTO>(userWithRoles);
        }
        public async Task<string> GetNewToken(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);

            if (user != null && user.RefreshTokenExpiryTime > DateTime.Now)
            {
                return GenerateJWT(await MapUserToDTO(user));
            }

            return string.Empty;
        }

        private string GenerateJWT(UserDTO user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var role in user.Roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(string email)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var refreshToken = Convert.ToBase64String(randomBytes);

            var user = await _userManager.FindByEmailAsync(email);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            var result = await _userManager.UpdateAsync(user);

            return refreshToken;
        }

        public async Task<BaseResponse> UserRegister(RegisterDTO model)
        {
            BaseResponse response = new();

            ApplicationUser user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result1 = await _userManager.CreateAsync(user, model.Password);
            var result2 = await _userManager.AddToRoleAsync(user, AppConstants.Roles.USER);

            if (result1.Succeeded && result2.Succeeded)
            {
                response.Message = "User created successfully";
            }
            else if (!result1.Succeeded)
            {
                string message = $"Registering user {model.Email} " + CollectErrors(result1);
                _logger.LogError(message);

                response.Succeeded = false;
                response.Message = message;
            }

            else
            {
                string message = $"Adding roles to user {model.Email} " + CollectErrors(result2);
                _logger.LogError(message);

                response.Succeeded = false;
                response.Message = message;
            }

            return response;
        }

        private static string CollectErrors(IdentityResult result) // helper
        {
            StringBuilder sb = new("failed with the following errors: ");
            foreach (var item in result.Errors)
            {
                sb.Append(item.Code + " " + item.Description);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> UserExistsById(string id)
        {
            return await _userManager.FindByIdAsync(id) != null;
        }

        public async Task<BaseResponse> DeleteMyAccountAsync(string userEmail)
        {
            var response = new BaseResponse();
            
            var userToDelete = await _userManager.FindByEmailAsync(userEmail);

            userToDelete.IsDeleted = true;
            await _userManager.UpdateSecurityStampAsync(userToDelete);

            await _context.SaveChangesAsync();

            return response;
        }
    }
}
