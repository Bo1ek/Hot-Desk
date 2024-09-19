using FluentValidation;
using SoftwareMind.Application.Common.DTOs;
using SoftwareMind.Application.Common.Helpers;

namespace SoftwareMind.Application.Common.Validator;
public class ReservationDateValidator : AbstractValidator<CreateReservationForMultipleDaysDto> 
{
    public ReservationDateValidator()
    {
        RuleFor(d => d.StartDate)
            .GreaterThanOrEqualTo((DateTimeHelper.SetTimeToStartOfDay(DateTime.Now)))
            .WithMessage("Start date must be greater than current date.")
            .LessThanOrEqualTo(d => DateTimeHelper.SetTimeToEndOfDay(d.EndDate))
            .WithMessage("Start date must be set before the end date.");
        RuleFor(d => d.EndDate)
            .GreaterThanOrEqualTo(d => DateTimeHelper.SetTimeToStartOfDay(d.StartDate))
            .WithMessage("End date must be set after the start date.")
            .GreaterThanOrEqualTo((DateTimeHelper.SetTimeToEndOfDay(DateTime.Now)))
            .WithMessage("End date must be greater than current date.")
            .LessThan(d => d.StartDate.AddDays(8))
            .WithMessage("Reservation cannot be longer than 7 days!");
    }
}
