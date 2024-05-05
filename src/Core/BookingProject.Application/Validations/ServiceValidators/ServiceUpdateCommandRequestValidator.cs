using BookingProject.Application.Features.Commands.ServiceCommands.ServiceCreateCommands;
using BookingProject.Application.Features.Commands.ServiceCommands.ServiceUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.ServiceValidators;

public class ServiceUpdateCommandRequestValidator : AbstractValidator<ServiceUpdateCommandRequest>
{
    public ServiceUpdateCommandRequestValidator()
    {
        RuleFor(x=>x.Id).NotNull().NotEmpty();
        RuleFor(x => x.IsDeactive).NotNull();
        RuleFor(x => x.ServiceName).NotEmpty().NotNull().MaximumLength(50);
    }
}
