using Cars.BLL.Services.Interfaces;
using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cars.BLL.Services.PhotoUrls
{
    public class PhotoUrlService : IPhotoUrlService
    {
        private readonly IPhotoUrlRepository _photoUrlRepository;

        public PhotoUrlService(IPhotoUrlRepository photoUrlRepository)
        {
            _photoUrlRepository = photoUrlRepository;
        }

        public async Task<PhotoUrl> CheckPropAsync(string photoUrl)
        {
            var result = await _photoUrlRepository.CheckPropAsync(photoUrl);

            return result;
        }

        public async Task<List<PhotoUrl>> CheckPhotoUrlListAsync(string[] photoUrlArray)
        {
            var result = await _photoUrlRepository.CheckPhotoUrlListAsync(photoUrlArray);

            return result;
        }
    }
}
