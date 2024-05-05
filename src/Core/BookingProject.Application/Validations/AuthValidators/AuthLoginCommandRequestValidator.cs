using BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;
using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;
using FluentValidation;

namespace BookingProject.Application.Validations;

public class AuthLoginCommandRequestValidator : AbstractValidator<AuthLoginCommandRequest>
{
    public AuthLoginCommandRequestValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().NotNull().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().NotNull().MaximumLength(50);
    }
}