using BookingProject.Application.Features.Commands.TypeCommands.TypeCreateCommands;
using BookingProject.Application.Features.Commands.TypeCommands.TypeUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.TypeValidators;

public class TypeUpdateCommandRequestValidator : AbstractValidator<TypeUpdateCommandRequest>
{
    public TypeUpdateCommandRequestValidator()
    {
        RuleFor(x=>x.Id).NotNull().NotEmpty();
        RuleFor(x => x.IsDeactive).NotNull();
        RuleFor(x => x.TypeName).NotEmpty().NotNull().MaximumLength(50);
    }
}
