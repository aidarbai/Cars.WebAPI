using Cars.BLL.Services.Interfaces;
using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Cars.BLL.Services.BodyTypes
{
    public class BodyTypeService : IBodyTypeService
    {
        private readonly IBodyTypeRepository _bodyTypeRepository;

        public BodyTypeService(IBodyTypeRepository bodyTypeRepository)
        {
            _bodyTypeRepository = bodyTypeRepository;
        }

        public async Task<BodyType> CheckPropAsync(string bodyType)
        {
            var result = await _bodyTypeRepository.CheckPropAsync(bodyType);

            return result;
        }
    }
}
