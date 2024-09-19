using FluentValidation.TestHelper;
using SoftwareMind.Application.Common.DTOs;
using SoftwareMind.Application.Common.Helpers;
using SoftwareMind.Application.Common.Validator;

namespace SoftwareMind.UnitTests.Validators
{
    public class ReservationDateValidatorTests
    {
        private readonly ReservationDateValidator _validator;

        public ReservationDateValidatorTests()
        {
            _validator = new ReservationDateValidator();
        }

        [Fact]
        public void StartDate_LessThanCurrentDate_ShouldHaveValidationError()
        {
            // Arrange
            var reservation = new CreateReservationForMultipleDaysDto
            {
                StartDate = DateTimeHelper.SetTimeToStartOfDay(DateTime.Now.AddDays(-1)),
                EndDate = DateTimeHelper.SetTimeToEndOfDay(DateTime.Now.AddDays(1))
            };

            // Act
            var result = _validator.TestValidate(reservation);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.StartDate)
                  .WithErrorMessage("Start date must be greater than current date.");
        }

        [Fact]
        public void StartDate_GreaterThanEndDate_ShouldHaveValidationError()
        {
            // Arrange
            var reservation = new CreateReservationForMultipleDaysDto
            {
                StartDate = DateTimeHelper.SetTimeToStartOfDay(DateTime.Now.AddDays(2)),
                EndDate = DateTimeHelper.SetTimeToEndOfDay(DateTime.Now.AddDays(1))
            };

            // Act 
            var result = _validator.TestValidate(reservation);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.StartDate)
                  .WithErrorMessage("Start date must be set before the end date.");
        }

        [Fact]
        public void EndDate_LessThanStartDate_ShouldHaveValidationError()
        {
            // Arrange
            var reservation = new CreateReservationForMultipleDaysDto
            {
                StartDate = DateTimeHelper.SetTimeToStartOfDay(DateTime.Now.AddDays(1)),
                EndDate = DateTimeHelper.SetTimeToEndOfDay(DateTime.Now.AddDays(-1))
            };

            // Act 
            var result = _validator.TestValidate(reservation);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.EndDate)
                  .WithErrorMessage("End date must be set after the start date.");
        }

        [Fact]
        public void EndDate_LessThanCurrentDate_ShouldHaveValidationError()
        {
            // Arrange
            var reservation = new CreateReservationForMultipleDaysDto
            {
                StartDate = DateTimeHelper.SetTimeToStartOfDay(DateTime.Now.AddDays(1)),
                EndDate = DateTimeHelper.SetTimeToEndOfDay(DateTime.Now.AddDays(-1))
            };

            // Act
            var result = _validator.TestValidate(reservation);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.EndDate)
                  .WithErrorMessage("End date must be greater than current date.");
        }

        [Fact]
        public void ReservationDuration_LongerThan7Days_ShouldHaveValidationError()
        {
            // Arrange
            var reservation = new CreateReservationForMultipleDaysDto
            {
                StartDate = DateTimeHelper.SetTimeToStartOfDay(DateTime.Now),
                EndDate = DateTimeHelper.SetTimeToEndOfDay(DateTime.Now.AddDays(8))
            };

            // Act  
            var result = _validator.TestValidate(reservation);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.EndDate)
                  .WithErrorMessage("Reservation cannot be longer than 7 days!");
        }

        [Fact]
        public void ValidReservation_ShouldNotHaveValidationError()
        {
            // Arrange
            var reservation = new CreateReservationForMultipleDaysDto
            {
                StartDate = DateTimeHelper.SetTimeToStartOfDay(DateTime.Now.AddDays(1)),
                EndDate = DateTimeHelper.SetTimeToEndOfDay(DateTime.Now.AddDays(2))
            };

            // Act
            var result = _validator.TestValidate(reservation);
            // Assert
            result.ShouldNotHaveValidationErrorFor(r => r.StartDate);
            result.ShouldNotHaveValidationErrorFor(r => r.EndDate);
        }
    }
}
