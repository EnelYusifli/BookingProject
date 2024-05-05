using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageCreateCommands;
using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.StaffLanguageValidators;

public class StaffLanguageUpdateCommandRequestValidator : AbstractValidator<StaffLanguageUpdateCommandRequest>
{
    public StaffLanguageUpdateCommandRequestValidator()
    {
        RuleFor(x=>x.Id).NotNull().NotEmpty();
        RuleFor(x => x.IsDeactive).NotNull();
        RuleFor(x => x.StaffLanguageName).NotEmpty().NotNull().MaximumLength(50);
    }
}
