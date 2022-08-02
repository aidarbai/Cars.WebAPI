using AutoMapper;
using Cars.BLL.Services.Interfaces;
using Cars.COMMON.DTOs;
using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Cars;
using Cars.DAL.Models;
using Cars.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Cars
{
    public class CarsService : ICarsService
    {
        private readonly ICarRepository _carRepository;

        public CarsService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        public async Task<CarVm> CreateCarAsync(Car car, string userEmail)
        {
            var result = await _carRepository.CreateCarAsync(car, userEmail);

            return result.Data;
        }
        public async Task<PaginatedResponse<CarVm>> GetCarsPaginatedAsync(SortAndPageCarModel model)
        {
            var result = await _carRepository.GetCarsPaginatedAsync(model);

            return Mapper.Map<PaginatedResponse<CarVm>>(result);
        }

        public async Task<IEnumerable<CarVm>> GetAllCarsAsync()
        {
            var result = await _carRepository.GetAllAsync();

            return Mapper.Map<IEnumerable<CarVm>>(result);
        }
        public async Task<CarVm> GetCarByIdAsync(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            return Mapper.Map<CarVm>(car);
        }
        public async Task<BaseResponse> UpdateCarAsync(CarUpdateDTO car, string userEmail)
        {
            return await _carRepository.UpdateCarAsync(car, userEmail);
        }

        public bool CarExistsById(int id)
        {
            return _carRepository.CarExistsById(id);
        }

        public bool CarExistsByVin(string vin)
        {
            return _carRepository.CarExistsByVin(vin);
        }

        public async Task<string[]> GetAllVinsAsync()
        {
            return await _carRepository.GetAllVinsAsync();
        }

        public async Task<BaseResponse> DeleteCarAsync(int id, string userEmail)
        {
            var result = await _carRepository.DeleteCarAsync(id, userEmail);

            return result;
        }
    }
}
