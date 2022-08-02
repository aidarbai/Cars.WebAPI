using Cars.COMMON.DTOs;
using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Cars;
using Cars.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Interfaces
{
    public interface ICarsService
    {
        bool CarExistsById(int id);

        bool CarExistsByVin(string vin);

        Task<CarVm> CreateCarAsync(Car car, string userEmail);

        Task<PaginatedResponse<CarVm>> GetCarsPaginatedAsync(SortAndPageCarModel model);

        Task<IEnumerable<CarVm>> GetAllCarsAsync();


        Task<CarVm> GetCarByIdAsync(int id);
        
        Task<string[]> GetAllVinsAsync();
        
        Task<BaseResponse> UpdateCarAsync(CarUpdateDTO carUpdate, string userEmail);
        
        Task<BaseResponse> DeleteCarAsync(int id, string userEmail);
    }
}