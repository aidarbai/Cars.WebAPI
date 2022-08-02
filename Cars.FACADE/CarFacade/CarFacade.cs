using AutoMapper;
using Cars.BLL.Services.Import;
using Cars.BLL.Services.Interfaces;
using Cars.COMMON.DTOs;
using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Cars;
using Cars.DAL.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.FACADE.CarFacade
{
    public class CarFacade : ICarFacade
    {
        private readonly ILogger<CarFacade> _logger;
        private readonly ICarsService _carService;
        private readonly IImportService _importService;
        private readonly IBodyTypeService _bodyTypeService;
        private readonly IColorService _colorService;
        private readonly IMakeService _makeService;
        private readonly IModelService _modelService;
        private readonly IPhotoUrlService _photoUrlService;
        public CarFacade(
            ILogger<CarFacade> logger,
            ICarsService carService,
            IImportService importService,
            IBodyTypeService bodyTypeService,
            IColorService colorService,
            IMakeService makeService,
            IModelService modelService,
            IPhotoUrlService photoUrlService)
        {
            _logger = logger;
            _carService = carService;
            _importService = importService;
            _bodyTypeService = bodyTypeService;
            _colorService = colorService;
            _makeService = makeService;
            _modelService = modelService;
            _photoUrlService = photoUrlService;
            _importService = importService;
            _logger = logger;
        }

        public async Task<CarVm> CreateCarAsync(CarImportDTO car, string userEmail)
        {
            var newCar = Mapper.Map<Car>(car);
            newCar.BodyType = await _bodyTypeService.CheckPropAsync(car.BodyType);
            newCar.Color = await _colorService.CheckPropAsync(car.DisplayColor);
            var make = await _makeService.CheckPropAsync(car.Make);
            newCar.Model = await _modelService.CheckModelAsync(car.Model, make);
            if (car.PhotoUrls?.Length > 0)
                newCar.PhotoUrls = await _photoUrlService.CheckPhotoUrlListAsync(car.PhotoUrls);

            var result = await _carService.CreateCarAsync(newCar, userEmail);

            return result;
        }

        public async Task<PaginatedResponse<CarVm>> GetCarsPaginatedAsync(SortAndPageCarModel model)
        {
            var result = await _carService.GetCarsPaginatedAsync(model);

            return result;
        }

        public async Task<IEnumerable<CarVm>> GetAllCarsAsync()
        {
            var result = await _carService.GetAllCarsAsync();

            return result;
        }

        public async Task<CarVm> GetCarByIdAsync(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            return car;
        }

        public async Task<BaseResponse> UpdateCarAsync(CarUpdateDTO car, string userEmail)
        {
            return await _carService.UpdateCarAsync(car, userEmail);
        }

        public bool CarExistsById(int id)
        {
            return _carService.CarExistsById(id);
        }

        public bool CarExistsByVin(string vin)
        {
            return _carService.CarExistsByVin(vin);
        }

        public async Task<string[]> GetAllVinsAsync()
        {
            return await _carService.GetAllVinsAsync();
        }

        public async Task<BaseResponse> DeleteCarAsync(int id, string userEmail)
        {
            var result = await _carService.DeleteCarAsync(id, userEmail);

            return result;
        }

        public async Task<BaseResponse> ImportCarsAsync(string userEmail)
        {
            int result = 0;
            var response = new BaseResponse();

            try
            {
                var importedCars = await GetListForImportAsync();
                if (importedCars.Count == 0)
                {
                    response.Message = "No cars have been uploaded";
                    return response;
                }

                var vins = await GetAllVinsAsync();

                importedCars = importedCars.FindAll(c => !vins.Contains(c.Vin));

                _logger.LogInformation("{count} new cars", importedCars.Count);

                foreach (var car in importedCars)
                {
                    //if (CarExistsByVin(car.Vin)) // array of vins as parameter, get new array - filter importedCars
                    //{
                    //    continue;
                    //}

                    var _ = await CreateCarAsync(car, userEmail);

                    if (_ != null)
                    {
                        result++;
                    }
                }
                response.Message = $"{result} cars have been uploaded to db";
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                response.Message = "Error while uploading new cars";
                response.Succeeded = false;
            }

            return response;
        }

        private async Task<List<CarImportDTO>> GetListForImportAsync()
        {
            var result = await _importService.GetListAsync();

            return result;
        }
    }
}
