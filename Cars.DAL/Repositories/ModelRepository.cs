using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories
{
    public class ModelRepository : GenericRepository<Model>, IModelRepository
    {
        public ModelRepository(DbContext.CarDbContext context) : base(context)
        {
        }

        public async Task<Model> CheckPropAsync(string modelName, Make make)
        {
            var response = await GetFirstOrDefaultAsync(b => b.Name == modelName);

            if (response != null)
                return response;

            var newModel = new Model
            {
                Name = modelName,
                Make = make
            };

            await AddAsync(newModel);

            return newModel;
        }
    }
}
