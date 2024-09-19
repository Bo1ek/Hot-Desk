using FluentValidation;
using SoftwareMind.Application.Common.DTOs;
namespace SoftwareMind.Application.Common.Validator;
public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City cannot be empty.")
            .NotNull()
            .WithMessage("City cannot be null.")
            .MinimumLength(3)
            .WithMessage("City must have more than 3 characters.");
    }
}
