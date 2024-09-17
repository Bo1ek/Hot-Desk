using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Repositories;
using SoftwareMind.Infrastructure.DTOs;
using Serilog;
using SoftwareMind.Infrastructure.Exceptions;

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
        if (_locationRepository.CheckIfExists(createDeskDto.LocationId))
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
        if (!_deskRepository.CheckIfReserved(deskId))
        {
            await _deskRepository.RemoveAsync(deskId);
            return NoContent();
        }
        return BadRequest();
    }
    //public async Task<ActionResult> ReserveDesk(int deskId)
    //{

    //}
}