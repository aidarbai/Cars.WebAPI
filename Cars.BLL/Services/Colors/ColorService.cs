using Cars.BLL.Services.Interfaces;
using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Colors
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public async Task<Color> CheckPropAsync(string color)
        {
            var result = await _colorRepository.CheckPropAsync(color);

            return result;
        }
    }
}
