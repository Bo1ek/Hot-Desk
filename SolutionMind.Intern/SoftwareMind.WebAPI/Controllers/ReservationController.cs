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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Reservation>> MakeReservationForOneDay(int deskId, string userId, DateTime reservationDay, CancellationToken cancellationToken = default)
        {
            await _reservationRepository.BookDeskForOneDay(deskId, userId, reservationDay, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task <ActionResult<Reservation>> UpdateDesk(int deskId, string userId, int reservationId, CancellationToken cancellationToken = default)
        {
            await _reservationRepository.UpdateDesk(deskId, userId, reservationId, cancellationToken);
            return Ok();
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Reservation>>> GetListOfReservations()
        {
            return await _reservationRepository.GetListOfReservations();
        }
    }
}
