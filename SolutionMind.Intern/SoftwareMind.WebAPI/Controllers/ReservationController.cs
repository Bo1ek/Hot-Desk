using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareMind.Application.Common.DTOs;
using SoftwareMind.Application.Common.Models;
using SoftwareMind.Application.Common.Validator;
using SoftwareMind.Infrastructure.Repositories;

namespace SoftwareMind.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        /// <summary>
        ///  Makes a reservation for at least one day but not more than 7 days
        /// </summary>
        /// <returns> 200StatusCode </returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     Post/api/Reservation/MakeReservationForMultipleDays
        ///     {
        ///         "deskId": 8,
        ///         "userId": "43ae890a-c4ee-4fe1-b2ba-b28ae8f5214e",
        ///         "startDate": "2024-09-21T21:57:54.130Z",
        ///         "endDate": "2024-09-21T21:57:54.130Z"
        ///     }
        /// 
        /// </remarks>
        /// <response code ="200"> Returns the OK response. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Reservation>> MakeReservationForMultipleDays(CreateReservationForMultipleDaysDto createReservationDto, CancellationToken cancellationToken = default)
        {
            var validation = new ReservationDateValidator();
            
            var validationResult = validation.Validate(createReservationDto);
            if (validationResult.IsValid)
            {
                await _reservationRepository.BookDeskForMultipleDays(createReservationDto, cancellationToken);
                return Ok();
            }

            return BadRequest(validationResult.Errors);

        }
        /// <summary>
        ///  Makes a reservation for one day 
        /// </summary>
        /// <returns> 200StatusCode </returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     Post/api/Reservation/MakeReservationForOneDay
        /// 
        /// </remarks>
        /// <response code ="200"> Returns the OK response. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Reservation>> MakeReservationForOneDay(int deskId, string userId, DateTime reservationDay, CancellationToken cancellationToken = default)
        {
            await _reservationRepository.BookDeskForOneDay(deskId, userId, reservationDay, cancellationToken);
            return Ok();
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
        public async Task <ActionResult<Reservation>> UpdateDesk(int deskId, string userId, int reservationId, CancellationToken cancellationToken = default)
        {
            await _reservationRepository.UpdateDesk(deskId, userId, reservationId, cancellationToken);
            return Ok();
        }
        /// <summary>
        /// Gets a list of all reservations
        /// </summary>
        /// <returns> 200StatusCode </returns>
        /// <remarks>
        /// </remarks>
        /// <response code ="200">Gets a list of all reservations. </response>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Reservation>>> GetListOfReservations()
        {
            return await _reservationRepository.GetListOfReservations();
        }
    }
}
