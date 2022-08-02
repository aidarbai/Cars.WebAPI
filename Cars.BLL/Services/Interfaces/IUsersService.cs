using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Users;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Interfaces
{
    public interface IUsersService
    {
        Task<PaginatedResponse<UserVm>> GetUserPaginatedAsync(SortAndPageUserModel model);
        Task<BaseResponse> EditUsersAsync(UserVm[] users);
        Task<BaseResponse> DeleteUserAsync(string[] ids);
        Task<BaseResponse> RestoreUserAsync(string[] ids);
        Task<BaseResponse> BanUserAsync(string[] ids);
        Task<BaseResponse> UnbanUserAsync(string[] ids);
    }
}
