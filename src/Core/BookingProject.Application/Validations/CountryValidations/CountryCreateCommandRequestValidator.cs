using BookingProject.Application.Features.Commands.CountryCommands.CountryCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.CountryValidators;

public class CountryCreateCommandRequestValidator:AbstractValidator<CountryCreateCommandRequest>
{
    public CountryCreateCommandRequestValidator()
    {
        RuleFor(x=>x.IsDeactive).NotNull();
        RuleFor(x=>x.CountryName).NotEmpty().NotNull().MaximumLength(50);
    }
}
