using Cars.BLL.ViewModels;
using Cars.BLL.DTOs.Auth;
using Cars.COMMON.Responses;
using System.Threading.Tasks;
using Cars.COMMON.DTOs;

namespace Cars.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseWithPayload<UserDTO>> Auth(LoginVm model);
        Task<BaseResponse> ChangePassword(ChangePasswordVm model);
        Task<BaseResponse> ResetPassword(ForgotPasswordVm model);
        Task<ResponseWithPayload<ForgotPasswordVm>> ForgotPassword(string email);
        Task<string> GetNewToken(string refreshToken);
        Task<BaseResponse> UserRegister(RegisterDTO model);
        Task<bool> UserExistsByEmail(string email);
        Task<bool> UserExistsById(string id);
        Task<BaseResponse> DeleteMyAccountAsync(string userEmail);
        
    }
}