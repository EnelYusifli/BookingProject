using BookingProject.Application.Features.Commands.TypeCommands.TypeCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.TypeValidators;

public class TypeCreateCommandRequestValidator:AbstractValidator<TypeCreateCommandRequest>
{
    public TypeCreateCommandRequestValidator()
    {
        RuleFor(x=>x.IsDeactive).NotNull();
        RuleFor(x=>x.TypeName).NotEmpty().NotNull().MaximumLength(50);
    }
}
