using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.ActivityValidators;

public class ActivityUpdateCommandRequestValidator : AbstractValidator<ActivityUpdateCommandRequest>
{
    public ActivityUpdateCommandRequestValidator()
    {
        RuleFor(x=>x.Id).NotNull().NotEmpty();
        RuleFor(x => x.IsDeactive).NotNull();
        RuleFor(x => x.ActivityName).NotEmpty().NotNull().MaximumLength(50);
    }
}
