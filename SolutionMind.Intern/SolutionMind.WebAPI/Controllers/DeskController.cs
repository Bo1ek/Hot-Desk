using Microsoft.AspNetCore.Mvc;
using SoftwareMind.Infrastructure.Repositories;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;

namespace SolutionMind.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeskController : Controller
{
    private readonly ILocationRepository _locationRepository;
    private readonly IDeskRepository _deskRepository;
    public DeskController(IDeskRepository deskRepository, ILocationRepository locationRepository)
    {
        _deskRepository = deskRepository;
        _locationRepository = locationRepository;
    }

    /// <summary>
    ///  Create a new desk
    /// </summary>
    /// <returns> 200StatusCode </returns>
    /// <remarks>
    /// Sample request: 
    /// 
    ///     Post/api/Desk
    ///     {
    ///         "locationId": 1
    ///     }
    ///         
    /// 
    /// </remarks>
    /// <response code ="201">Returns the newly created Desk. </response>
    /// <response code ="404">Returns not found status due to lack of Location to assign. </response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateDeskDto>> CreateDesk(CreateDeskDto createDeskDto)
    {
        if (_locationRepository.Exists(createDeskDto.LocationId))
        {
            await _deskRepository.CreateAsync(createDeskDto);
            return Ok(createDeskDto);
        }
        return NotFound();
    }

    /// <summary>
    /// Delete a desk by id
    /// </summary>
    /// <returns> 204StatusCode </returns>
    /// <remarks>
    /// Sample request: 
    /// 
    ///     Delete/api/desk/{deskId}
    /// 
    /// </remarks>
    /// <response code ="204">Returns No Content message.  </response>
    /// <response code ="404">Returns BadRequest response. " </response>
    [HttpDelete("{deskId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoveDesk(int deskId)
    {
        if (_deskRepository.IsAvailable(deskId) && _deskRepository.Exists(deskId))
        {
            await _deskRepository.RemoveAsync(deskId);
            return NoContent();
        }
        return BadRequest();
    }

    /// <summary>
    /// Update a Desks status to unavailable
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
    public async Task<ActionResult<Desk>> MakeUnavailable(int deskId, CancellationToken cancellationToken = default)
    {
        await _deskRepository.MakeUnavailable(deskId);
        return Ok();
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<Desk>>> GetAllAvailableDesks()
    {
        return await _deskRepository.getAllAvailableDesks();
    }
    [HttpGet("unavailable")]
    public async Task<ActionResult<List<Desk>>> GetAllUnavailableDesks()
    {
        return await _deskRepository.getAllUnavailableDesks();
    }
}