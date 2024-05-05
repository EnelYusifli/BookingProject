using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.StaffLanguageValidators;

public class StaffLanguageCreateCommandRequestValidator:AbstractValidator<StaffLanguageCreateCommandRequest>
{
    public StaffLanguageCreateCommandRequestValidator()
    {
        RuleFor(x=>x.IsDeactive).NotNull();
        RuleFor(x=>x.StaffLanguageName).NotEmpty().NotNull().MaximumLength(50);
    }
}
