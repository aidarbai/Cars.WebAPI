using Cars.API.Controllers;
using Cars.COMMON.Responses;
using Cars.COMMON.ViewModels.Cars;
using Cars.FACADE.CarFacade;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Cars.Tests
{
    [Collection("Fixture collection")]
    public class CarsControllerTests
    {
        private InMemoryContextFixture Fixture { get; }
        private CarFacade CarFacade { get; }
        private CarsController Controller { get; }

        private SortAndPageCarModel sortAndPageModel { get; } = new SortAndPageCarModel { PageSize = 20 };
        public CarsControllerTests(InMemoryContextFixture fixture)
        {
            Fixture = fixture;

            Controller = new CarsController(
                    Fixture.CarFacade,
               Fixture.DistributedCache,
                      ArrangeTestHelper.GetLogger<CarsController>());
        }

        [Fact]
        public async Task Get_Returns_Result()
        {
            var responseFromController = await Controller.GetAsync(sortAndPageModel);
            var okObject = responseFromController.Result as OkObjectResult;

            var paginatedResponse = okObject.Value as PaginatedResponse<CarVm>;
            var countFromResponse = paginatedResponse.Results.Count();
            var countFromDb = Fixture.Context.CarsV2.Count();

            //Assert.IsType<ActionResult<PaginatedResponse<CarVm>>>(result1);
            responseFromController.Should().BeOfType<ActionResult<PaginatedResponse<CarVm>>>();

            //Assert.Equal(countFromDb, countFromResponse);
            countFromResponse.Should().Be(countFromDb);
        }

        [Fact]
        public async Task Get_By_Id_Unknown_Id_Passed_Returns_Not_FoundResult()
        {
            var id = Fixture.Context.CarsV2.Last().Id;
            var notFoundResult = await Controller.GetCarAsync(id + 1);

            //Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
            notFoundResult.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Get_By_Id_Returns_Correct_Object()
        {
            var id = Fixture.Context.CarsV2.First().Id;
            var responseFromController = await Controller.GetCarAsync(id);

            var okObject = responseFromController.Result as OkObjectResult;
            var car = okObject.Value as CarVm;

            //Assert.IsType<OkObjectResult>(responseFromController.Result);
            responseFromController.Result.Should().BeOfType<OkObjectResult>();
            //Assert.Equal(id, car.Id);
            car.Id.Should().Be(id);
        }
    }
}
