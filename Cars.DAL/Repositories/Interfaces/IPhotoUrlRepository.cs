using Cars.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories.Interfaces
{
    public interface IPhotoUrlRepository : IGenericRepository<PhotoUrl>, IPropCheck<PhotoUrl>
    {
        Task<List<PhotoUrl>> CheckPhotoUrlListAsync(string[] photoUrlList);
    }
}
