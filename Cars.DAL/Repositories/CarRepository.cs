using Cars.COMMON.DTOs;
using Cars.DAL.DbContext;
using Cars.DAL.Helpers;
using Cars.DAL.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cars.COMMON.Constants;
using Microsoft.AspNetCore.Identity;
using Cars.COMMON.Responses;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Cars.COMMON.ViewModels.Cars;
using Cars.DAL.Repositories.Interfaces;

namespace Cars.DAL.Repositories
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CarRepository> _logger;
        public CarRepository(
            CarDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<CarRepository> logger) : base(context)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ResponseWithPayload<CarVm>> CreateCarAsync(Car car, string userEmail)
        {
            car.User = await _userManager.FindByEmailAsync(userEmail);

            await AddAsync(car);

            var response = new ResponseWithPayload<CarVm>();
            var createdCar = Find(c => c.Vin == car.Vin).FirstOrDefault();
            response.Data = Mapper.Map<CarVm>(createdCar);

            return response;
        }

        public async Task<PaginatedResponse<Car>> GetCarsPaginatedAsync(SortAndPageCarModel model)
        {
            var carsQuery = SortingHelper.GetSortedQuery(_context, model);

            var count = await _context.CarsV2.CountAsync();

            var result = new PaginatedResponse<Car>
            {
                ItemsCount = count,
                PageSize = model.PageSize,
                TotalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize)),
                PageNumber = model.PageNumber,
                Results = await carsQuery
                            .Skip((model.PageNumber - 1) * model.PageSize)
                            .Take(model.PageSize)
                            .Include(c => c.BodyType)
                            .Include(c => c.Color)
                            .Include(c => c.Model)
                            .ThenInclude(m => m.Make)
                            .Include(c => c.PhotoUrls)
                            .ToListAsync()

            };

            return result;
        }
        
        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.CarsV2
                                 .Include(c => c.BodyType)
                                 .Include(c => c.Color)
                                 .Include(c => c.Model)
                                 .ThenInclude(m => m.Make)
                                 .Include(c => c.PhotoUrls)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<BaseResponse> UpdateCarAsync(CarUpdateDTO car, string userEmail)
        {
            var response = new BaseResponse();

            try
            {
                var carToUpdate = await _context.Set<Car>().Include(c => c.User).FirstOrDefaultAsync(c => c.Id == car.Id);
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (await CheckUserCarAccess(carToUpdate.User.Email, user))
                {
                    Mapper.Map(car, carToUpdate);

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.Message = "Car was notupdated";
                response.Succeeded = false;
            }

            return response;
        }

        public async Task<bool> CheckUserCarAccess(string carUserEmail, ApplicationUser user)
        {
            return carUserEmail == user.Email
                                || (await _userManager.IsInRoleAsync(user, AppConstants.Roles.ADMIN)
                                || await _userManager.IsInRoleAsync(user, AppConstants.Roles.SUPER_ADMIN));
        }

        public bool CarExistsById(int id)
        {
            return _context.Set<Car>().Any(e => e.Id == id);
        }

        public bool CarExistsByVin(string vin)
        {
            return _context.Set<Car>().Any(e => e.Vin == vin);
        }

        public async Task<string[]> GetAllVinsAsync()
        {
            return await _context.Set<Car>().Select(c => c.Vin).ToArrayAsync();
        }

        public async Task<BaseResponse> DeleteCarAsync(int id, string userEmail)
        {
            var response = new BaseResponse();

            try
            {
                var carToDelete = await _context.CarsV2.Include(u => u.User).FirstOrDefaultAsync(d => d.Id == id);
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user != null && await CheckUserCarAccess(carToDelete.User.Email, user))
                {
                    carToDelete.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }

                else
                {
                    response.Succeeded = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.Succeeded = false;
                response.Message = "Car was not deleted";
            }

            return response;
        }
    }
}
