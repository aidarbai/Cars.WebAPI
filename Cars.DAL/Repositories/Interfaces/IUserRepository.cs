using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Users;
using Cars.DAL.Models;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginatedResponse<ApplicationUser>> GetUsersPaginatedAsync(SortAndPageUserModel model);
    }
}
