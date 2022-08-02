using Microsoft.Extensions.DependencyInjection;
using Cars.BLL.Services.Cars;
using Cars.BLL.Services.Import;
using Cars.BLL.Services.Auth;
using Cars.DAL.Repositories;
using Cars.DAL.Repositories.Interfaces;
using Cars.FACADE.CarFacade;
using Cars.BLL.Services.Interfaces;
using Cars.BLL.Services.BodyTypes;
using Cars.BLL.Services.Colors;
using Cars.BLL.Services.Makes;
using Cars.BLL.Services.Models;
using Cars.BLL.Services.PhotoUrls;
using Cars.BLL.Services.Users;
using Cars.BLL.Services.EmailSender;

namespace Cars.API.Helpers
{
    public static class DIHelper
    {
        public static void AddCarServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IImportService, ImportService>();

            services.AddTransient<ICarFacade, CarFacade>();
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IBodyTypeService, BodyTypeService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<IMakeService, MakeService>();
            services.AddTransient<IModelService, ModelService>();
            services.AddTransient<IPhotoUrlService, PhotoUrlService>();
            services.AddTransient<IEmailSender, EmailSender>();


            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBodyTypeRepository, BodyTypeRepository>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<IMakeRepository, MakeRepository>();
            services.AddTransient<IModelRepository, ModelRepository>();
            services.AddTransient<IPhotoUrlRepository, PhotoUrlRepository>();

            services.AddTransient<CarCreateService>();

        }
    }
}