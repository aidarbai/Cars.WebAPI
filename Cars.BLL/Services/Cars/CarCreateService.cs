using AutoMapper;
using Cars.COMMON.DTOs;
using Cars.DAL.DbContext;
using Cars.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Cars
{
    public class CarCreateService
    {
        private readonly ILogger<CarCreateService> _logger;
        private readonly CarDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public CarCreateService(
            ILogger<CarCreateService> logger,
            CarDbContext db, 
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public async Task<bool> UploadToDbAsync(CarImportDTO car, string userEmail)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var newCar = Mapper.Map<Car>(car);

                newCar.User = await _userManager.FindByEmailAsync(userEmail);

                var color = await _db.Colors.FirstOrDefaultAsync(c => c.Name == car.DisplayColor.ToLower().Trim());
                if (color == null)
                {
                    color = new Color()
                    {
                        Name = car.DisplayColor.ToLower().Trim()
                    };
                    await _db.Colors.AddAsync(color);
                }
                newCar.Color = color;

                var bodytype = await _db.Bodytypes.Where(c => c.Name == car.BodyType.ToLower().Trim()).FirstOrDefaultAsync();
                if (bodytype == null)
                {
                    bodytype = new BodyType()
                    {
                        Name = car.BodyType.ToLower().Trim()
                    };
                    await _db.Bodytypes.AddAsync(bodytype);
                }
                newCar.BodyType = bodytype;

                var model = await _db.Models.Where(c => c.Name == car.Model.ToLower().Trim()).FirstOrDefaultAsync();
                if (model == null)
                {
                    model = new Model()
                    {
                        Name = car.Model.ToLower().Trim()
                    };
                    await _db.Models.AddAsync(model);
                }
                newCar.Model = model;

                var make = await _db.Makes.Where(c => c.Name == car.Make.ToLower().Trim()).FirstOrDefaultAsync();
                if (make == null)
                {
                    make = new Make()
                    {
                        Name = car.Make.ToLower().Trim()
                    };
                    await _db.Makes.AddAsync(make);
                    await _db.SaveChangesAsync();
                }
                newCar.Model.Make = make;

                await _db.CarsV2.AddAsync(newCar);

                if (car.PhotoUrls?.Length > 0)
                {
                    foreach (var item in car.PhotoUrls)
                    {
                        var photoUrl = new PhotoUrl() { Path = item, SourceId = newCar.Id, SourceType = PhotoSourceType.Car };
                        await _db.PhotoUrls.AddAsync(photoUrl);
                        newCar.PhotoUrls.Add(photoUrl);
                    }
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                transaction.Rollback();
                return false;
            }

            return true;
        }

    }
}
