using BookingProject.Application.Features.Commands.ServiceCommands.ServiceCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.ServiceValidators;

public class ServiceCreateCommandRequestValidator:AbstractValidator<ServiceCreateCommandRequest>
{
    public ServiceCreateCommandRequestValidator()
    {
        RuleFor(x=>x.IsDeactive).NotNull();
        RuleFor(x=>x.ServiceName).NotEmpty().NotNull().MaximumLength(50);
    }
}
