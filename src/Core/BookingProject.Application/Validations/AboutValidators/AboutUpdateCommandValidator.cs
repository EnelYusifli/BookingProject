using BookingProject.Application.Features.Commands.AboutCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.AboutValidators;

public class AboutUpdateCommandValidator: AbstractValidator<AboutUpdateCommandRequest>
{
    public AboutUpdateCommandValidator()
    {
        RuleFor(x=>x.Story).NotNull().NotEmpty().MaximumLength(5000);
        RuleFor(x=>x.StoryTitle).NotNull().NotEmpty().MaximumLength(200);
    }
}
