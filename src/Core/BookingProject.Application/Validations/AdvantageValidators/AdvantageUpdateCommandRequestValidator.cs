using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageCreateCommands;
using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.AdvantageValidators;

public class AdvantageUpdateCommandRequestValidator : AbstractValidator<AdvantageUpdateCommandRequest>
{
    public AdvantageUpdateCommandRequestValidator()
    {
        RuleFor(x=>x.Id).NotNull().NotEmpty().GreaterThanOrEqualTo(1);
        RuleFor(x => x.IsDeactive).NotNull();
        RuleFor(x => x.AdvantageName).NotEmpty().NotNull().MaximumLength(200);
    }
}
