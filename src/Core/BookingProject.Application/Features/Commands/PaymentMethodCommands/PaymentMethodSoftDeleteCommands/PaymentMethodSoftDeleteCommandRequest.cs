using MediatR;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodSoftDeleteCommands;

public class PaymentMethodSoftDeleteCommandRequest:IRequest<PaymentMethodSoftDeleteCommandResponse>
{
    public required int Id { get; set; }
}
