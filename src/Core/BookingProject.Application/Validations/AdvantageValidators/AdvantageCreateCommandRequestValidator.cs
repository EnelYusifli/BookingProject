using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.AdvantageValidators;

public class AdvantageCreateCommandRequestValidator:AbstractValidator<AdvantageCreateCommandRequest>
{
    public AdvantageCreateCommandRequestValidator()
    {
        RuleFor(x=>x.IsDeactive).NotNull();
        RuleFor(x=>x.AdvantageName).NotEmpty().NotNull().MaximumLength(200);
    }
}
