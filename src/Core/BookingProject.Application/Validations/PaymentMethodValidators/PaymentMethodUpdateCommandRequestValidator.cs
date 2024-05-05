using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodCreateCommands;
using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodUpdateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.PaymentMethodValidators;

public class PaymentMethodUpdateCommandRequestValidator : AbstractValidator<PaymentMethodUpdateCommandRequest>
{
    public PaymentMethodUpdateCommandRequestValidator()
    {
        RuleFor(x=>x.Id).NotNull().NotEmpty();
        RuleFor(x => x.IsDeactive).NotNull();
        RuleFor(x => x.PaymentMethodName).NotEmpty().NotNull().MaximumLength(50);
    }
}
