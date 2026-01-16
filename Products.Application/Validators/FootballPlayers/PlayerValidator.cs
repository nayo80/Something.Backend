using FluentValidation;
using Products.Application.Dtos.Cars;
using Products.Application.Dtos.FootballPlayers;

namespace Products.Application.Validators.FootballPlayers;

public class PlayerValidator : AbstractValidator<RequestFootballPlayer>
{
    public PlayerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotNull().NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.FootballClub)
            .NotNull().NotEmpty()
            .MaximumLength(100);
        
    }
}