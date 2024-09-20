using Microsoft.AspNetCore.Mvc;
using Moq;
using SoftwareMind.Application.Common.DTOs;
using SoftwareMind.Application.Common.Models;
using SoftwareMind.Application.Common.Validator;
using SoftwareMind.Infrastructure.Repositories;
using SoftwareMind.WebAPI.Controllers;

namespace SoftwareMind.UnitTests.WebAPI.Controllers;

public class ReservationControllerTests
{
    private readonly Mock<IReservationRepository> _mockReservationRepository;

    private readonly ReservationController _controller;
    private readonly CancellationToken cancellationToken = new CancellationTokenSource().Token;

    public ReservationControllerTests()
    {
        _mockReservationRepository = new Mock<IReservationRepository>();
        _controller = new ReservationController(_mockReservationRepository.Object);
    }

    [Fact]
    public async Task MakeReservationForMultipleDays_ValidData_ShouldReturnOk()
    {
        // Arrange
        var createReservationDto = new CreateReservationForMultipleDaysDto
        {
            DeskId = 1,
            UserId = "1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var validation = new ReservationDateValidator();
        var validationResult = validation.Validate(createReservationDto);

        _mockReservationRepository
            .Setup(repo => repo.BookDeskForMultipleDays(It.IsAny<CreateReservationForMultipleDaysDto>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.MakeReservationForMultipleDays(createReservationDto);

        // Assert
        Assert.True(validationResult.IsValid);
        var okResult = Assert.IsType<OkResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        _mockReservationRepository.Verify(repo => repo.BookDeskForMultipleDays(createReservationDto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MakeReservationForMultipleDays_InvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var createReservationDto = new CreateReservationForMultipleDaysDto
        {
            DeskId = 1,
            UserId = "1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(-1)
        };

        var validation = new ReservationDateValidator();
        var validationResult = validation.Validate(createReservationDto);

        // Act
        var result = await _controller.MakeReservationForMultipleDays(createReservationDto);

        // Assert
        Assert.False(validationResult.IsValid);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badRequestResult.StatusCode);
        _mockReservationRepository.Verify(repo => repo.BookDeskForMultipleDays(It.IsAny<CreateReservationForMultipleDaysDto>(), cancellationToken), Times.Never);
    }

    [Fact]
    public async Task MakeReservationForOneDay_ValidData_ShouldReturnOk()
    {
        // Arrange
        var deskId = 1;
        var userId = "1";
        var reservationDay = DateTime.Now;

        _mockReservationRepository
            .Setup(repo => repo.BookDeskForOneDay(deskId, userId, reservationDay, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.MakeReservationForOneDay(deskId, userId, reservationDay);

        // Assert
        var okResult = Assert.IsType<OkResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        _mockReservationRepository.Verify(repo => repo.BookDeskForOneDay(deskId, userId, reservationDay, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDesk_ValidData_ShouldReturnOk()
    {
        // Arrange
        var deskId = 1;
        var userId = "1";
        var reservationId = 1;
        var expectedReservation = new Reservation { Id = reservationId, DeskId = deskId, UserId = userId, StartDate = DateTime.Now };
        _mockReservationRepository
            .Setup(repo => repo.UpdateDesk(deskId, userId, reservationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedReservation);

        // Act
        var result = await _controller.UpdateDesk(deskId, userId, reservationId);

        // Assert
        var okResult = Assert.IsType<OkResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        _mockReservationRepository.Verify(repo => repo.UpdateDesk(deskId, userId, reservationId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetListOfReservations_ShouldReturnListOfReservations()
    {
        // Arrange
        var reservations = new List<Reservation>
        {
            new() { Id = 1, DeskId = 1, UserId = "1", StartDate = DateTime.Now },
            new() { Id = 2, DeskId = 2, UserId = "2", StartDate = DateTime.Now }
        };

        _mockReservationRepository
            .Setup(repo => repo.GetListOfReservations())
            .ReturnsAsync(reservations);

        // Act
        var result = await _controller.GetListOfReservations();

        // Assert
        var okResult = Assert.IsType<ActionResult<List<Reservation>>>(result);
        var returnValue = Assert.IsType<List<Reservation>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
        _mockReservationRepository.Verify(repo => repo.GetListOfReservations(), Times.Once);
    }
}
