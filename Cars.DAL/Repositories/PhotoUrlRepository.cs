using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories
{
    public class PhotoUrlRepository : GenericRepository<PhotoUrl>, IPhotoUrlRepository
    {
        public PhotoUrlRepository(DbContext.CarDbContext context) : base(context)
        {
        }

        public async Task<PhotoUrl> CheckPropAsync(string path)
        {
            var response = await GetFirstOrDefaultAsync(b => b.Path == path);

            if (response != null)
                return response;

            var newBodyType = new PhotoUrl
            {
                Path = path,
                // SourceId = newCar.Id, ???
                SourceType = PhotoSourceType.Car
            };

            await AddAsync(newBodyType);

            return newBodyType;
        }
        public async Task<List<PhotoUrl>> CheckPhotoUrlListAsync(string[] photoUrlArray)
        {
            var photoUrlList = new List<PhotoUrl>();

            if (photoUrlArray.Length > 0)
            {
                for (int i = 0; i < photoUrlArray.Length; i++)
                {
                    var photoUrl = await CheckPropAsync(photoUrlArray[i]);
                    photoUrlList.Add(photoUrl);
                }
            }

            return photoUrlList;
        }
    }
}
