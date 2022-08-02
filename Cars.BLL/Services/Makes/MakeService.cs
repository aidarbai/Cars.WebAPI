using Cars.BLL.Services.Interfaces;
using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Makes
{
    public class MakeService : IMakeService
    {
        private readonly IMakeRepository _makeRepository;

        public MakeService(IMakeRepository makeRepository)
        {
            _makeRepository = makeRepository;
        }

        public async Task<Make> CheckPropAsync(string make)
        {
            var result = await _makeRepository.CheckPropAsync(make);

            return result;
        }
    }
}
