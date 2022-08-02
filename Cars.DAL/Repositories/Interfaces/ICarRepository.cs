using Cars.COMMON.DTOs;
using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Cars;
using Cars.DAL.Models;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories.Interfaces
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        Task<ResponseWithPayload<CarVm>> CreateCarAsync(Car car, string userEmail);
        
        Task<PaginatedResponse<Car>> GetCarsPaginatedAsync(SortAndPageCarModel model);
        
        Task<Car> GetCarByIdAsync(int id);

        Task<BaseResponse> UpdateCarAsync(CarUpdateDTO car, string userEmail);
        
        bool CarExistsById(int id);

        bool CarExistsByVin(string vin);

        Task<string[]> GetAllVinsAsync();

        public Task<BaseResponse> DeleteCarAsync(int id, string userEmail);
    }
}
