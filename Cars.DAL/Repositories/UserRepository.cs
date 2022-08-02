using Cars.COMMON.Responses;
using Cars.DAL.DbContext;
using Cars.DAL.Helpers;
using Cars.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Cars.COMMON.ViewModels.Users;
using System.Linq;
using static Cars.COMMON.Constants.AppConstants;
using System.Linq.Expressions;
using Cars.DAL.Repositories.Interfaces;
using System.Collections.Generic;

namespace Cars.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedResponse<ApplicationUser>> GetUsersPaginatedAsync(SortAndPageUserModel model) // make base class
        {
            var usersQuery = _userManager.Users.AsQueryable();

            usersQuery = model.Order == Order.ASC
                ? usersQuery.OrderBy(model.SortBy)
                : usersQuery.OrderByDescending(model.SortBy);

            var count = await _userManager.Users.CountAsync();

            var users = await usersQuery
                            .IgnoreQueryFilters()
                            .Skip((model.PageNumber - 1) * model.PageSize)
                            .Take(model.PageSize)
                            .Include(c => c.Cars)
                            .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                            .AsSplitQuery()
                            .AsNoTracking()
                            .ToListAsync();

            var result = new PaginatedResponse<ApplicationUser>
            {
                ItemsCount = count,
                PageSize = model.PageSize,
                TotalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize)),
                PageNumber = model.PageNumber,
                Results = users
            };

            return result;
        }
    }
}
