using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;
using FluentValidation;

namespace BookingProject.Application.Validations;

public class AuthRegisterCommandRequestValidator : AbstractValidator<AuthRegisterCommandRequest>
{
    public AuthRegisterCommandRequestValidator()
    {
        RuleFor(x=>x.UserName).NotNull().NotEmpty().MaximumLength(256);
        RuleFor(x=>x.Email).EmailAddress().NotNull().NotEmpty().MaximumLength(256);
        RuleFor(x=>x.FirstName).NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x=>x.LastName).NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Password).NotNull().NotEmpty().MaximumLength(50);
        RuleFor(x=>x.ConfirmPassword).NotNull().NotEmpty().MaximumLength(50);
        RuleFor(x=>x.Birthdate).NotNull().NotEmpty();
    }
}
