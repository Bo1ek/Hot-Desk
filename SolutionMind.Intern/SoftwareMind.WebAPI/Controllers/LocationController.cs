using Microsoft.AspNetCore.Mvc;
using Serilog;
using SoftwareMind.Application.Common.DTOs;
using SoftwareMind.Application.Common.Exceptions;
using SoftwareMind.Application.Common.Models;
using SoftwareMind.Application.Common.Validator;
using SoftwareMind.Infrastructure.Repositories;

namespace SoftwareMind.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        /// <summary>
        ///  Create a new location
        /// </summary>
        /// <returns> 200StatusCode </returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     Post/api/location/CreateLocation
        ///     {
        ///         "city": "Katowice"
        ///     }
        ///         
        /// 
        /// </remarks>
        /// <response code ="200"> Returns the newly created Location with the OK response. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Location>> CreateLocation(CreateLocationDto createLocationDto)
        {
            var validator = new CreateLocationValidator();
            var validationResult = validator.Validate(createLocationDto);
            if (validationResult.IsValid)
            {
                await _locationRepository.CreateAsync(createLocationDto);
                return Ok(createLocationDto);
            }
            return BadRequest(validationResult.Errors);
        }

        /// <summary>
        /// Update a location
        /// </summary>
        /// <returns> 200StatusCode </returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     Put/api/location/UpdateLocation
        ///     {
        ///         "id" : 1,
        ///         "city": "Warszawa"
        ///     }
        ///         
        /// 
        /// </remarks>
        /// <response code ="200">Returns the updated Location with it's Id. </response>
        /// <response code ="500">Returns error message from Validator. </response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationDto>> UpdateLocation(LocationDto locationDto)
        {
            var validator = new UpdateLocationValidator();
            var validationResult = validator.Validate(locationDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _locationRepository.UpdateAsync(locationDto);
            return Ok(locationDto);
        }

        /// <summary>
        /// Delete a location by id
        /// </summary>
        /// <returns> 204StatusCode </returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     Delete/api/location/RemoveLocation/{locationId}
        /// 
        /// </remarks>
        /// <response code ="204">Returns No Content response.  </response>
        /// <response code ="404">Returns message "Location with id not found." </response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveLocation(int locationId)
        {
            try
            {
                await _locationRepository.RemoveAsync(locationId);
                return NoContent();
            }
            catch (LocationNotFoundException ex)
            {
                Log.Error(ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
