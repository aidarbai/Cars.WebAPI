using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cars.COMMON.DTOs;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Cars.COMMON.Constants;
using Cars.COMMON.Responses;
using Cars.BLL.Helpers;
using Cars.COMMON.ViewModels.Cars;
using Cars.FACADE.CarFacade;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Security.Claims;

namespace Cars.API.Controllers
{
    [Authorize(Roles = AppConstants.Roles.Groups.ADMINSGROUP)]
    [Route(Routes.Controllers.CARS)]
    [ApiController]
    [ApiVersion(AppConstants.ApiAttributes.VERSION)]
    [Produces(AppConstants.ApiAttributes.CONTENTTYPE)]
    public class CarsController : ControllerBase
    {
        private readonly ICarFacade _carFacade;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CarsController> _logger;

        public CarsController(
            ICarFacade carFacade,
            IDistributedCache distributedCache,
            ILogger<CarsController> logger)
        {
            _carFacade = carFacade;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        [HttpPost(Routes.Methods.CREATE)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation("Create new car")]

        public async Task<ActionResult<CarVm>> PostAsync([FromBody] CarImportDTO car)
        {
            //if (!ModelState.IsValid || car == null)
            //{
            //    _logger.LogError("Car object sent from client is null.");
            //    return BadRequest(new BaseResponse { Succeeded = false, Message = "Invalid data" });
            //}

            if (_carFacade.CarExistsByVin(car.Vin))
            {
                _logger.LogError($"Car with VIN {car.Vin} already exists.");
                return BadRequest(new BaseResponse { Succeeded = false, Message = "Car with this VIN already exists" });
            }

            string userEmail = User.Identity.Name;

            var createdCar = await _carFacade.CreateCarAsync(car, userEmail);

            if (createdCar == null)
            {
                HttpStatusCodeHelper.Error500();
            }

            return CreatedAtAction(nameof(GetCarAsync), new { id = createdCar.Id }, createdCar);
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Get all cars paginated")]

        public async Task<ActionResult<PaginatedResponse<CarVm>>> GetAsync([FromQuery] SortAndPageCarModel model)
        {
            string cacheKey = model.ToString();
            string serializedCarList;
            PaginatedResponse<CarVm> cars = new();
            // move to service
            try
            {
                var redisCarList = await _distributedCache.GetAsync(cacheKey);
                if (redisCarList != null)
                {
                    serializedCarList = Encoding.UTF8.GetString(redisCarList);
                    cars = JsonConvert.DeserializeObject<PaginatedResponse<CarVm>>(serializedCarList);
                }
                else
                {
                    cars = await _carFacade.GetCarsPaginatedAsync(model);
                    serializedCarList = JsonConvert.SerializeObject(cars);
                    redisCarList = Encoding.UTF8.GetBytes(serializedCarList);
                    var options = new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                        SlidingExpiration = TimeSpan.FromMinutes(1),
                    };
                    await _distributedCache.SetAsync(cacheKey, redisCarList, options);
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                cars = await _carFacade.GetCarsPaginatedAsync(model);
            }

            return Ok(cars);
        }

        [AllowAnonymous]
        [HttpGet(Routes.Methods.ID)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Get car by id")]
        public async Task<ActionResult<CarVm>> GetCarAsync(int id)
        {
            var car = await _carFacade.GetCarByIdAsync(id);

            return car == null ?
                        NotFound(new BaseResponse { Succeeded = false }) :
                        Ok(car);
        }

        [HttpPut(Routes.Methods.UPDATE)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Update car")]
        public async Task<IActionResult> PutAsync([FromBody] CarUpdateDTO carUpdate)
        {
            //if (!ModelState.IsValid)
            //{
            //    _logger.LogError("Invalid car object sent from client.");
            //    return BadRequest(new BaseResponse { Succeeded = false, Message = "Invalid data" });
            //}

            if (!_carFacade.CarExistsById(carUpdate.Id))
            {
                return NotFound();
            }

            string userEmail = User.Identity.Name;
            var response = await _carFacade.UpdateCarAsync(carUpdate, userEmail);
            if (!response.Succeeded)
            {
                return string.IsNullOrEmpty(response.Message) ?
                 Forbid() : BadRequest(response);
            }

            return Ok();
        }

        [HttpDelete(Routes.Methods.DELETE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Delete car")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!_carFacade.CarExistsById(id))
            {
                return NotFound();
            }

            //string userEmail = User.Identity.Name;
            string userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var response = await _carFacade.DeleteCarAsync(id, userEmail);

            if (!response.Succeeded)
            {
                return string.IsNullOrEmpty(response.Message) ?
                 Forbid() : BadRequest(response);
            }

            return NoContent();
        }

        [HttpGet(Routes.Methods.IMPORT)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Import cars")]

        public async Task<ActionResult<IEnumerable<CarVm>>> ImportAsync()
        {
            string userEmail = User.Identity.Name;
            var result = await _carFacade.ImportCarsAsync(userEmail);

            return Ok(result.Message);
        }
    }
}
