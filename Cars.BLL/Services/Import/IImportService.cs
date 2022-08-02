using Cars.COMMON.DTOs;
using Cars.COMMON.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Import
{
    public interface IImportService
    {
        Task<List<CarImportDTO>> GetListAsync();
    }
}