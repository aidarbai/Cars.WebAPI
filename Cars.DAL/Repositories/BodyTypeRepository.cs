using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories
{
    public class BodyTypeRepository : GenericRepository<BodyType>, IBodyTypeRepository
    {

        public BodyTypeRepository(DbContext.CarDbContext context) : base(context)
        {
        }

        public async Task<BodyType> CheckPropAsync(string bodyType)
        {
            var response = await GetFirstOrDefaultAsync(b => b.Name == bodyType);

            if (response != null)
                return response;

            var newBodyType = new BodyType
            {
                Name = bodyType
            };

            await AddAsync(newBodyType);

            return newBodyType;
        }
    }
}
