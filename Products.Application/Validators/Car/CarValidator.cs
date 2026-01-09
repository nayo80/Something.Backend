using FluentValidation;
using Products.Application.Dtos.Cars;

namespace Products.Application.Validators.Car;

public class CarValidator : AbstractValidator<RequestCarDto>
{
    public CarValidator()
    {
        RuleFor(x => x.Brand)
            .NotNull().NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Model)
            .NotNull().NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ReleaseDate)
            .NotNull()
            .GreaterThan(new DateTime(1753, 1, 1))
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Release date must be between 1/1/1753 and today.");
    }
}