using Cars.COMMON.Constants;
using Cars.COMMON.Responses;
using Cars.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Cars.BLL.Helpers
{
    public static class UserRolesHelper
    {
        public static async Task<BaseResponse> SeedAsync(IServiceProvider serviceProvider)
        {
            BaseResponse response = new();
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                IdentityResult roleResult;

                bool allRolesExisted = true;
                foreach (var roleName in AppConstants.Roles.Groups.ROLES)
                {
                    var roleExists = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        allRolesExisted = false;
                        roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName));
                        //roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
                if (allRolesExisted)
                {
                    response.Message = "All user roles already exist in database";
                }
                else
                {
                    response.Message = "User roles have been added successfully";
                }

                string email = configuration["superAdminEmail"];
                
                var user = await userManager.FindByEmailAsync(email);
                
                if (user == null)
                {
                    ApplicationUser superAdmin = new();
                    superAdmin.Email = email;
                    superAdmin.UserName = email;
                    string password = configuration["superAdminPassword"];
                    superAdmin.FirstName = configuration["superAdminName"];
                    superAdmin.LastName = configuration["superAdminLastName"];

                    var newUser = await userManager.CreateAsync(superAdmin, password);

                    if (newUser.Succeeded)
                    {
                        var newUserRole = userManager.AddToRoleAsync(superAdmin, AppConstants.Roles.SUPER_ADMIN);
                        response.Message += ", superadmin has been added";
                    }
                }
                else
                {
                    response.Message += ", superadmin already exists in database";
                }
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
            }

            return response;

        }
    }
}
