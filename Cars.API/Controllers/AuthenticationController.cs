using Cars.BLL.ViewModels;
using Cars.COMMON.Constants;
using Cars.COMMON.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Cars.BLL.DTOs.Auth;
using Cars.BLL.Services.Interfaces;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Cars.COMMON.ViewModels.Users;

namespace Cars.API.Controllers
{
    [Route(Routes.Controllers.AUTH)]
    [ApiController]
    [ApiVersion(AppConstants.ApiAttributes.VERSION)]
    [Produces(AppConstants.ApiAttributes.CONTENTTYPE)]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly string baseUrl;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAuthService authservice,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _logger = logger;
            _authservice = authservice;
            _emailSender = emailSender;
            baseUrl = configuration["URLS:FrontEnd"];
        }

        [HttpPost]
        [Route(Routes.Methods.LOGIN)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Login")]
        public async Task<IActionResult> Login([FromBody] LoginVm model)
        {
            var result = await _authservice.Auth(model);
            if (result.Succeeded)
            {
                AddTokenToCookies(result.Data.JWTToken);
                return Ok(result.Data);
            }

            return BadRequest(new BaseResponse { Succeeded = false, Message = result.Message });
        }

        [HttpPost]
        [Route(Routes.Methods.CHANGEPASSWORD)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Change Password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordVm model)
        {
            var result = await _authservice.ChangePassword(model);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(new BaseResponse { Succeeded = false, Message = result.Message });
        }

        [HttpPost]
        [Route(Routes.Methods.GETNEWTOKEN)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("GetNewToken")]
        public async Task<IActionResult> GetNewToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogError("Invalid refreshToken sent from client.");
                return BadRequest(new BaseResponse { Succeeded = false, Message = "Invalid data" });
            }

            var token = await _authservice.GetNewToken(refreshToken);

            if (!string.IsNullOrEmpty(token))
            {
                AddTokenToCookies(token);

                return Ok();
            }

            return BadRequest(new BaseResponse { Succeeded = false, Message = "Invalid refresh token" });
        }

        [HttpPost]
        [Route(Routes.Methods.RESETPASSWORD)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordVm model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Code))
            {
                _logger.LogError("Invalid email or code sent from client.");
                return BadRequest(error: new BaseResponse { Succeeded = false, Message = "Invalid data" });
            }

            var response = await _authservice.ResetPassword(model);

            if (response.Succeeded)
            {
                return Ok();
            }

            return BadRequest(error: new BaseResponse { Succeeded = false, Message = response.Message });
        }

        [HttpPost]
        [Route(Routes.Methods.FORGOTPASSWORD)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("Invalid email sent from client.");
                return BadRequest(error: new BaseResponse { Succeeded = false, Message = "Invalid data" });
            }

            string url = baseUrl + Routes.Paths.RESETPASSWORD;

            var response = await _authservice.ForgotPassword(email);

            if (response.Succeeded)
            {
                await _emailSender.SendEmailAsync(
                        email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(url + response.Data.Code)}'>clicking here</a>.");

                return Ok(new BaseResponse { Succeeded = true, Message = "Email message has been sent." });
            }

            return BadRequest(error: new BaseResponse { Succeeded = false, Message = response.Message });
        }

        [HttpPost]
        [Route(Routes.Methods.REGISTER)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid register object sent from client.");
                return BadRequest(new BaseResponse { Succeeded = false, Message = "Invalid data" });
            }

            if (await _authservice.UserExistsByEmail(model.Email))
            {
                _logger.LogError($"DB contains user with email {model.Email}");
                return StatusCode(StatusCodes.Status409Conflict, new BaseResponse { Succeeded = false, Message = "This username is taken" });
            }

            var response = await _authservice.UserRegister(model);

            if (!response.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpDelete(Routes.Methods.DELETE)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Delete my account")]
        public async Task<IActionResult> DeleteMyAccountAsync()
        {
            string userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var response = await _authservice.DeleteMyAccountAsync(userEmail);

            return Ok(new BaseResponse { Succeeded = true, Message = "User was deleted" });
        }

        private void AddTokenToCookies(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Path = "/"
            };
            Response.Cookies.Append("jwt-token", token, cookieOptions);
        }
    }
}