using MediatR;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodDeleteCommands;

public class PaymentMethodDeleteCommandRequest:IRequest<PaymentMethodDeleteCommandResponse>
{
    public required int Id { get; set; }
}
