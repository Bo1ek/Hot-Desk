﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SoftwareMind.Infrastructure.DTOs;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Repositories;
using SoftwareMind.Infrastructure.Validator;

namespace SolutionMind.WebAPI.Controllers
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
        public async Task<ActionResult<Reservation>> MakeReservationForMultipleDays(CreateReservationForMultipleDaysDto createReservationDto)
        {
            var validation = new DateValidator();
            var validationResult = validation.Validate(createReservationDto);
            if (validationResult.IsValid)
            {
                await _reservationRepository.BookDeskForMultipleDays(createReservationDto);
                return Ok();
            }
            return BadRequest(validationResult.Errors);

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Reservation>> MakeReservationForOneDay(int deskId, string userId, DateTime reservationDay)
        {
            await _reservationRepository.BookDeskForOneDay(deskId, userId, reservationDay);
            return Ok();
        }
    }
}
