using Microsoft.AspNetCore.Mvc;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Repositories;

namespace SolutionMind.WebAPI.Controllers
{
    [Route ("api/[controller]")]
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Location>> CreateLocation(LocationDto locationDto)
        {
            await _locationRepository.CreateAsync(locationDto);
            return Ok(locationDto);
        }

        /// <summary>
        /// Update a location
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Location>> UpdateLocation(LocationDto locationDto)
        {
            await _locationRepository.UpdateAsync(locationDto);
            return Ok(locationDto);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpDelete("{locationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveLocation(int locationId)
        {
            await _locationRepository.RemoveAsync(locationId);
            return Ok();
        }

    }
}
