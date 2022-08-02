using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories
{
    public class MakeRepository : GenericRepository<Make>, IMakeRepository
    {
        public MakeRepository(DbContext.CarDbContext context) : base(context)
        {
        }

        public async Task<Make> CheckPropAsync(string makeName)
        {
            var response = await GetFirstOrDefaultAsync(b => b.Name == makeName);

            if (response != null)
                return response;

            var newMake = new Make
            {
                Name = makeName
            };

            await AddAsync(newMake);

            return newMake;
        }
    }
}
