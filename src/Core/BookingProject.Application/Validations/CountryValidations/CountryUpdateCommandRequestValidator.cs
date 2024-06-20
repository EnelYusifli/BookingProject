using BookingProject.Application.Features.Commands.CountryCommands.CountryCreateCommands;
using BookingProject.Application.Features.Commands.CountryCommands.CountryUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.CountryValidators;

public class CountryUpdateCommandRequestValidator : AbstractValidator<CountryUpdateCommandRequest>
{
    public CountryUpdateCommandRequestValidator()
    {
        RuleFor(x=>x.Id).NotNull().NotEmpty();
        RuleFor(x => x.IsDeactive).NotNull();
        RuleFor(x => x.CountryName).NotEmpty().NotNull().MaximumLength(50);
    }
}
