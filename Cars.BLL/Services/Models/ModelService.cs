using Cars.BLL.Services.Interfaces;
using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Models
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;

        public ModelService(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<Model> CheckModelAsync(string model, Make make)
        {
            var result = await _modelRepository.CheckPropAsync(model, make);

            return result;
        }
    }
}
