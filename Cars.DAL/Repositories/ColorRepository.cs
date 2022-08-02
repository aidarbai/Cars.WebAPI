using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(DbContext.CarDbContext context) : base(context)
        {
        }

        public async Task<Color> CheckPropAsync(string colorName)
        {
            var response = await GetFirstOrDefaultAsync(b => b.Name == colorName);

            if (response != null)
                return response;

            var newColor = new Color
            {
                Name = colorName
            };

            await AddAsync(newColor);

            return newColor;
        }
    }
}
