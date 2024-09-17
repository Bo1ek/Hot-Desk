using Microsoft.AspNetCore.Mvc;
using Serilog;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Exceptions;
using SoftwareMind.Infrastructure.Repositories;
using SoftwareMind.Infrastructure.Validator;

namespace SolutionMind.WebAPI.Controllers
{
    [Route("api/[controller]")]
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
        ///     Post/api/location
        ///     {
        ///         "city": "Katowice"
        ///     }
        ///         
        /// 
        /// </remarks>
        /// <response code ="201">Returns the newly created Location. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Location>> CreateLocation(CreateLocationDto createLocationDto)
        {
            var validator = new CreateLocationValidator();
            var validationResult = validator.Validate(createLocationDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _locationRepository.CreateAsync(createLocationDto);
            return Ok(createLocationDto);
        }

        /// <summary>
        /// Update a location
        /// </summary>
        /// <returns> 200StatusCode </returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     Put/api/location
        ///     {
        ///         "id" : 1,
        ///         "city": "Warszawa"
        ///     }
        ///         
        /// 
        /// </remarks>
        /// <response code ="201">Returns the updated Location with it's Id. </response>
        /// <response code ="400">Returns errror message from Validator. </response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        ///     Delete/api/location/{locationId}
        /// 
        /// </remarks>
        /// <response code ="204">Returns No Content message.  </response>
        /// <response code ="404">Returns message "Location with id {id} not found." </response>
        [HttpDelete("{locationId}")]
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
