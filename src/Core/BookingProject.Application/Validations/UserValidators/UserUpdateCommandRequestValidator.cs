using BookingProject.Application.Features.Commands.UserCommands.UserUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.UserValidators;

public class UserUpdateCommandRequestValidator : AbstractValidator<UserUpdateCommandRequest>
{
    public UserUpdateCommandRequestValidator()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x => x.UserName).NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).MaximumLength(100);
    }
}
