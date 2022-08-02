using Cars.BLL.Services.Interfaces;
using Cars.COMMON.Constants;
using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Cars;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Threading.Tasks;
using Cars.COMMON.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;

namespace Cars.API.Controllers
{
    [Authorize(Roles = AppConstants.Roles.Groups.ADMINSGROUP)]
    [Route(Routes.Controllers.USERS)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userservice;
        
        public UsersController(
            IUsersService userservice,
            IAuthService authservice,
            ILogger<CarsController> logger)
        {
            _userservice = userservice;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Get all users paginated")]

        public async Task<ActionResult<PaginatedResponse<CarVm>>> GetAsync([FromQuery] SortAndPageUserModel model)
        {
            var result = await _userservice.GetUserPaginatedAsync(model);
            
            return Ok(result);
        }

        [HttpPost(Routes.Methods.EDIT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Edit users")]
        public async Task<IActionResult> EditAsync(UserVm[] users)
        {
            var result = await _userservice.EditUsersAsync(users);
            return Ok(new BaseResponse { Succeeded = true, Message = "User(s) was(ere) updated" });
        }

        [HttpPost(Routes.Methods.DELETE)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Delete user")]
        public async Task<IActionResult> DeleteAsync(string[] ids)
        {
            var response = await _userservice.DeleteUserAsync(ids);
            return Ok(new BaseResponse { Succeeded = true, Message = "User(s) was(ere) deleted" });
        }

        [HttpPost(Routes.Methods.RESTORE)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Restore user")]
        public async Task<IActionResult> RestoreUserAsync(string []ids)
        {
            var response = await _userservice.DeleteUserAsync(ids);
            return Ok(new BaseResponse { Succeeded = true, Message = "User(s) was(ere) restored" });
        }

        [HttpPost(Routes.Methods.BAN)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Ban user")]
        public async Task<IActionResult> BanUserAsync(string[] ids)
        {
            var response = await _userservice.BanUserAsync(ids);
            return Ok(new BaseResponse { Succeeded = true, Message = "User(s) was(ere) banned" });
        }

        [HttpPost(Routes.Methods.UNBAN)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Unban user")]
        public async Task<IActionResult> UnbanUserAsync(string[] ids)
        {
            var response = await _userservice.UnbanUserAsync(ids);
            return Ok(new BaseResponse { Succeeded = true, Message = "User(s) was(ere) unbanned" });
        }
    }
}
