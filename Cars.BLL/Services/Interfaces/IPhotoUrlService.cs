using Cars.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Interfaces
{
    public interface IPhotoUrlService
    {
        Task<PhotoUrl> CheckPropAsync(string photoUrl);
        Task<List<PhotoUrl>> CheckPhotoUrlListAsync(string[] photoUrlArray);
    }
}
