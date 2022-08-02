using AutoMapper;
using Cars.BLL.Services.Auth;
using Cars.BLL.Services.Interfaces;
using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Cars;
using Cars.COMMON.ViewModels.Users;
using Cars.DAL.DbContext;
using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CarDbContext _context;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IUserRepository userRepository,
                            UserManager<ApplicationUser> userManager,
                            CarDbContext context,
                            ILogger<UsersService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<PaginatedResponse<UserVm>> GetUserPaginatedAsync(SortAndPageUserModel model)
        {
            var result = await _userRepository.GetUsersPaginatedAsync(model);

            return Mapper.Map<PaginatedResponse<UserVm>>(result);
        }

        public async Task<BaseResponse> EditUsersAsync(UserVm[] users)
        {
            var response = new BaseResponse();


            var time = DateTime.Now.ToUniversalTime();

            var usersToDelete = await _context.Users.Where(u => users.Select(x => x.Id).Contains(u.Id)).ToListAsync();

            foreach (var user in users)
            {
                var oldUser = usersToDelete.FirstOrDefault(u => u.Id == user.Id);

                oldUser.IsDeleted = user.IsDeleted;

                if (user.IsBanned && oldUser.BannedTime == null)
                {
                    oldUser.BannedTime = time;
                }

                if (!user.IsBanned && oldUser.BannedTime != null)
                {
                    oldUser.BannedTime = null;
                }
            }

            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<BaseResponse> DeleteUserAsync(string[] ids)
        {
            var response = new BaseResponse();

            var usersToDelete = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

            usersToDelete.ForEach(user => user.IsDeleted = true);

            LogMismatch(await _context.SaveChangesAsync(), ids.Length);

            return response;
        }

        public async Task<BaseResponse> RestoreUserAsync(string[] ids)
        {
            var response = new BaseResponse();

            var usersToUnDelete = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

            usersToUnDelete.ForEach(user => user.IsDeleted = false);

            LogMismatch(await _context.SaveChangesAsync(), ids.Length);

            return response;
        }

        public async Task<BaseResponse> BanUserAsync(string[] ids)
        {
            var response = new BaseResponse();

            var usersToBan = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

            var time = DateTime.UtcNow;
            usersToBan.ForEach(user => user.BannedTime = time);

            LogMismatch(await _context.SaveChangesAsync(), ids.Length);

            return response;
        }

        public async Task<BaseResponse> UnbanUserAsync(string[] ids)
        {
            var response = new BaseResponse();

            var usersToUnban = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

            usersToUnban.ForEach(user => user.BannedTime = null);

            LogMismatch(await _context.SaveChangesAsync(), ids.Length);

            return response;
        }

        private void LogMismatch(int result, int expected)
        {
            if (result != expected)
            {
                _logger.LogError("{result} changes to DB were made while {expected} were expected", result, expected);
            }
        }
    }
}
