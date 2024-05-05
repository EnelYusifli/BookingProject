using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodCreateCommands;
using FluentValidation;

namespace BookingProject.Application.Validations.PaymentMethodValidators;

public class PaymentMethodCreateCommandRequestValidator:AbstractValidator<PaymentMethodCreateCommandRequest>
{
    public PaymentMethodCreateCommandRequestValidator()
    {
        RuleFor(x=>x.IsDeactive).NotNull();
        RuleFor(x=>x.PaymentMethodName).NotEmpty().NotNull().MaximumLength(50);
    }
}
