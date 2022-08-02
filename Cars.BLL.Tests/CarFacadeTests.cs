using Cars.COMMON.DTOs;
using Cars.COMMON.ViewModels.Cars;
using Cars.FACADE.CarFacade;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Cars.Tests
{
    [Collection("Fixture collection")]
    public class CarFacadeTests
    {
        private InMemoryContextFixture Fixture { get; }

        private CarFacade Sut { get; }

        private SortAndPageCarModel sortAndPageModel { get; } = new SortAndPageCarModel { PageSize = 20 };

        private const string ADMINEMAIL = "superadmin@email.com";

        private const int VIN_LENGTH= 17;

        private const string IMPORTED_VIN = "2C3CDZFJ3LH151103";

        public CarFacadeTests(InMemoryContextFixture fixture)
        {
            Fixture = fixture;
            Sut = Fixture.CarFacade;
        }

        [Fact]
        public void Car_Facade_Should_Find_Car_By_Vin_Correctly()
        {
            var vin = Fixture.Context.CarsV2.ToList().First().Vin;
            
            Sut.CarExistsByVin(vin).Should().BeTrue("this car exists in DB");
        }

        [Fact]
        public async Task Car_Facade_Should_Create_Car_Correctly()
        {
            var newCar = ArrangeTestHelper.GetCarImportDTO();

            //Assert.False(Sut.CarExistsByVin(newCar.Vin));
            Sut.CarExistsByVin(newCar.Vin).Should().BeFalse("this car should not exist in DB yet");

            var createdCar = await Sut.CreateCarAsync(newCar, ADMINEMAIL);

            //Assert.Equal(newCar.Vin, createdCar.Vin);
            createdCar.Vin.Should().Be(newCar.Vin);
            //Assert.True(Sut.CarExistsByVin(newCar.Vin));
            Sut.CarExistsByVin(newCar.Vin).Should().BeTrue("this car should be present in DB now");
        }

        [Fact]
        public async Task Car_Facade_Should_Return_Cars_Correctly()
        {
            var listOfCars = await Sut.GetCarsPaginatedAsync(sortAndPageModel);
            var resultFromSut = listOfCars.Results.ToList();
            var carsFromDB = Fixture.Context.CarsV2.ToList();
            var vins = resultFromSut.Select(c => c.Vin).ToArray();

            //Assert.Equal(carsFromDB.Count, resultFromSut.Count);
            resultFromSut.Count.Should().Be(carsFromDB.Count);

            //Array.ForEach(vins, vin => Assert.Equal(VIN_LENGTH, vin.Length));
            vins.Should().AllSatisfy(vin => vin.Length.Should().Be(VIN_LENGTH));
        }

        [Fact]
        public async Task Car_Facade_Should_Return_Car_By_Id_Correctly()
        {
            var response = await Sut.GetCarsPaginatedAsync(sortAndPageModel);
            var firstCar = response.Results.First();
            var car = await Sut.GetCarByIdAsync(firstCar.Id);

            //Assert.NotNull(car);
            car.Should().NotBeNull();
            //Assert.Equal(VIN_LENGTH, car.Vin.Length);
            car.Vin.Length.Should().Be(VIN_LENGTH);
        }

        [Fact]
        public async Task Car_Service_Should_UpdateCar_Correctly()
        {
            //Arrange
            var response = await Sut.GetCarsPaginatedAsync(sortAndPageModel);
            var firstCar = response.Results.First();
            var car = await Sut.GetCarByIdAsync(firstCar.Id);
            const int newPrice = 123;

            //Mapping carVm to CarUpdateDTO
            var carForUpdate = new CarUpdateDTO()
            {
                Id = car.Id,
                Price = car.Price
            };
            //Updating price
            carForUpdate.Price = newPrice;


            //Act
            var resultOfCarUpdate = await Sut.UpdateCarAsync(carForUpdate, ADMINEMAIL);
            var updatedCar = await Sut.GetCarByIdAsync(carForUpdate.Id);

            //Assert
            //Assert.NotEqual(newPrice, car.Price);
            car.Price.Should().NotBe(newPrice);
            //Assert.True(resultOfCarUpdate.Succeeded);
            resultOfCarUpdate.Succeeded.Should().BeTrue();
            //Assert.Equal(carForUpdate.Price, updatedCar.Price);
            updatedCar.Price.Should().Be(carForUpdate.Price);
        }

        [Fact]
        public async Task CarServiceShouldDeleteCarCorrectly()
        {
            var response= await Sut.GetCarsPaginatedAsync(sortAndPageModel);
            var car = response.Results.Last();

            var resultOfDeletion = await Sut.DeleteCarAsync(car.Id, ADMINEMAIL);
            var carExists = await Sut.GetCarByIdAsync(car.Id);

            //Assert.True(resultOfDeletion.Succeeded);
            resultOfDeletion.Succeeded.Should().BeTrue();
            //Assert.Null(carExists);
            carExists.Should().BeNull();
        }

        [Fact]
        public async Task CarServiceShouldImportCarsCorrectly()
        {
            var cars = Fixture.Context.CarsV2.ToList();
            var initCount = cars.Count;

            var result = await Sut.ImportCarsAsync(ADMINEMAIL);
            cars = Fixture.Context.CarsV2.ToList();

            //Assert.Equal(initCount + 2, cars.Count);
            cars.Count.Should().Be(initCount + 2);
            //Assert.True(result.Succeeded);
            result.Succeeded.Should().BeTrue();
            //Assert.Contains(cars, car => car.Vin == "2C3CDZFJ3LH151103");
            cars.Should().Contain(car => car.Vin == IMPORTED_VIN);
        }
    }
}
