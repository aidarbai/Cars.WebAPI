using Cars.COMMON.DTOs;
using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Cars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cars.FACADE.CarFacade
{
    public interface ICarFacade
    {
        Task<CarVm> CreateCarAsync(CarImportDTO car, string userEmail);
        
        Task<PaginatedResponse<CarVm>> GetCarsPaginatedAsync(SortAndPageCarModel model);

        Task<IEnumerable<CarVm>> GetAllCarsAsync();

        Task<CarVm> GetCarByIdAsync(int id);
        
        Task<BaseResponse> UpdateCarAsync(CarUpdateDTO car, string userEmail);
        
        bool CarExistsById(int id);
        
        bool CarExistsByVin(string vin);

        Task<string[]> GetAllVinsAsync();

        Task<BaseResponse> DeleteCarAsync(int id, string userEmail);
        
        Task<BaseResponse> ImportCarsAsync(string userEmail);
    }
}