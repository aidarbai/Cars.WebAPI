using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cars.COMMON.Constants;
using Cars.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Cars.DAL.DbContext;
using Cars.FACADE.CarFacade;
using Cars.DAL.Repositories.Interfaces;
using Cars.BLL.Services.BodyTypes;
using Cars.BLL.Services.Import;
using Cars.BLL.Services.Makes;
using Cars.BLL.Services.PhotoUrls;
using Cars.BLL.Services.Cars;
using Cars.DAL.Repositories;
using Cars.BLL.Services.Colors;
using Cars.BLL.Services.Models;
using Cars.COMMON.DTOs;
using Microsoft.Extensions.Caching.Distributed;

namespace Cars.Tests
{
    public static class ArrangeTestHelper
    {
        private const string ADMINEMAIL = "superadmin@email.com";

        public static ILogger<T> GetLogger<T>() where T : class
        {
            var serviceProvider = new ServiceCollection()
                                                .AddLogging()
                                                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            return factory.CreateLogger<T>();
        }
        public static IDistributedCache GetDistributedCache()
        {
            var serviceProvider = new ServiceCollection()
                                                .AddDistributedMemoryCache()
                                                .BuildServiceProvider();
            
            return serviceProvider.GetService<IDistributedCache>();
        }

        public static Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var _userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

            var user = GetUser();

            var tcs1 = new TaskCompletionSource<ApplicationUser>();
            tcs1.SetResult(user);
            _userManager.Setup(x => x.FindByEmailAsync("superadmin@email.com")).Returns(tcs1.Task);


            var tcs2 = new TaskCompletionSource<bool>();
            tcs2.SetResult(true);
            _userManager.Setup(x => x.IsInRoleAsync(user, AppConstants.Roles.SUPER_ADMIN)).Returns(tcs2.Task);

            return _userManager;
        }

        public static ApplicationUser GetUser()
        {
            return new()
            {
                Email = ADMINEMAIL,
                UserName = ADMINEMAIL,
                FirstName = "Super",
                LastName = "Admin",
            };
        }

        public static CarImportDTO GetCarImportDTO()
        {
            return new CarImportDTO()
            {
                Id = 238674471,
                Vin = "5J6RW1H50KA018226",
                Year = 2019,
                PriceUnformatted = 23977,
                MileageUnformatted = 89659,
                City = "Margate",
                PrimaryPhotoUrl = "https://auto.dev/images/forsale/2022/03/09/22/18/2019_honda_cr-v-pic-5310913567299889195-1024x768.jpeg",
                Condition = "used",
                DisplayColor = "Gunmetal Metallic",
                Make = "Honda",
                Model = "CR-V",
                BodyType = "suv",
                PhotoUrls = new string[] {
                    "https://auto.dev/image/fetch/s--N4dktbyZ--/c_fill,f_auto,q_auto,w_320/https://static.cargurus.com/images/forsale/2022/03/09/22/18/2019_honda_cr-v-pic-5310913567299889195-1024x768.jpeg",
                    "https://auto.dev/image/fetch/s--JTtDduOx--/c_fill,f_auto,q_auto,w_320/https://static.cargurus.com/images/forsale/2022/03/09/22/18/2019_honda_cr-v-pic-7462306682117433771-1024x768.jpeg",
                    "https://auto.dev/image/fetch/s--y5870Fcr--/c_fill,f_auto,q_auto,w_320/https://static.cargurus.com/images/forsale/2022/03/09/22/18/2019_honda_cr-v-pic-1641440423351142392-1024x768.jpeg"
                }
            };
        }
        public static CarDbContext SeedDb(CarDbContext context)
        {
            context.CarsV2.Add(new()
            {
                ExternalId = 39,
                Vin = "2C3CDZFJ6LH104227",
                Year = 2020,
                Price = 57999,
                Mileage = 16013,
                City = "Margate",
                PrimaryPhotoUrl = "https://auto.dev/images/forsale/2021/06/29/19/23/2020_dodge_challenger-pic-3166061963558229316-1024x768.jpeg",
                Condition = "used",
                Model = new Model() { Name = "Challenger", Make = new Make() { Name = "Dodge"} },
                Color = new Color() { Name = "Sublime Pearlcoat" },
                BodyType = new BodyType() { Name = "sedan"},
                User = GetUser(),
            });

            context.CarsV2.Add(new()
            {
                ExternalId = 38,
                Vin = "2C3CDZFJ6KH625575",
                Year = 2019,
                Price = 56499,
                Mileage = 22347,
                City = "Margate",
                PrimaryPhotoUrl = "https://auto.dev/images/forsale/2021/06/22/03/18/2019_dodge_challenger-pic-3022365507893257151-1024x768.jpeg",
                Condition = "used",
                Model = new Model() { Name = "Challenger", Make = new Make() { Name = "Dodge" } },
                Color = new Color() { Name = "Sublime Pearlcoat" },
                BodyType = new BodyType() { Name = "sedan" },
                User = GetUser(),
            });

            context.SaveChanges();

            return context;
        }
        public static IConfiguration GetConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> { { "CarsDBListings", "https://test.domain" } };

            return new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }
        public static HttpClient GetHClient()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""records"": [
                    {
                    ""id"": 39,
                    ""vin"": ""2C3CDZFJ6LH104227"",
                    ""year"": 2020,
                    ""priceUnformatted"": 57999,
                    ""mileageUnformatted"": 16013,
                    ""city"": ""Margate"",
                    ""primaryPhotoUrl"": ""https://auto.dev/images/forsale/2021/06/29/19/23/2020_dodge_challenger-pic-3166061963558229316-1024x768.jpeg"",
                    ""condition"": ""used"",
                      },
                    {
                    ""id"": 38,
                    ""vin"": ""2C3CDZFJ6KH625575"",
                    ""year"": 2019,
                    ""priceUnformatted"": 56499,
                    ""mileageUnformatted"": 22347,
                    ""city"": ""Margate"",
                    ""primaryPhotoUrl"": ""https://auto.dev/images/forsale/2021/06/22/03/18/2019_dodge_challenger-pic-3022365507893257151-1024x768.jpeg"",
                    ""condition"": ""used""
                      },
                    {
                    ""id"": 23,
                    ""vin"": ""1GYS3BKJ3HR171960"",
                    ""year"": 2017,
                    ""priceUnformatted"": 52999,
                    ""mileageUnformatted"": 41975,
                    ""city"": ""Margate"",
                    ""primaryPhotoUrl"": ""https://auto.dev/images/forsale/2021/08/13/23/36/2017_cadillac_escalade-pic-5377648330135415874-1024x768.jpeg"",
                    ""condition"": ""used"",
                    ""displayColor"": ""Crystal White Tricoat"",
                    ""make"": ""Cadillac"",
                    ""model"": ""Escalade"",
                    ""bodyType"": ""suv""
                      },
                    {
                    ""id"": 25,
                    ""vin"": ""2C3CDZFJ3LH151103"",
                    ""year"": 2020,
                    ""priceUnformatted"": 46499,
                    ""mileageUnformatted"": 9795,
                    ""city"": ""Margate"",
                    ""primaryPhotoUrl"": ""https://auto.dev/images/forsale/2021/07/13/03/15/2020_dodge_challenger-pic-6259409633593488211-1024x768.jpeg"",
                    ""condition"": ""used"",
                    ""displayColor"": ""Granite Pearlcoat"",
                    ""make"": ""Dodge"",
                    ""model"": ""Challenger"",
                    ""bodyType"": ""coupe""
                      }]}")
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            return new HttpClient(handlerMock.Object);
        }

        public static CarFacade GetCarFacade(ICarRepository carRepository, CarDbContext context)
        {
            var logger = GetLogger<CarFacade>();
            var carService = new CarsService(carRepository);
            var importService = new ImportService(GetConfiguration(), GetHClient());
            var bodyTypeService = new BodyTypeService(new BodyTypeRepository(context));
            var colorService = new ColorService(new ColorRepository(context));
            var makeService = new MakeService(new MakeRepository(context));
            var modelService = new ModelService(new ModelRepository(context));
            var photoUrlService = new PhotoUrlService(new PhotoUrlRepository(context));

            return new CarFacade(
                logger,
                carService,
                importService,
                bodyTypeService,
                colorService,
                makeService,
                modelService,
                photoUrlService);
        }
    }
}
