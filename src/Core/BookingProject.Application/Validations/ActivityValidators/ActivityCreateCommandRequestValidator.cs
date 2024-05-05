using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.ActivityValidators;

public class ActivityCreateCommandRequestValidator:AbstractValidator<ActivityCreateCommandRequest>
{
    public ActivityCreateCommandRequestValidator()
    {
        RuleFor(x=>x.IsDeactive).NotNull();
        RuleFor(x=>x.ActivityName).NotEmpty().NotNull().MaximumLength(50);
    }
}
