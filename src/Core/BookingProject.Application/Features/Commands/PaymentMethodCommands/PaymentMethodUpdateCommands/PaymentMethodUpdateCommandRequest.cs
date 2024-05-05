using MediatR;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodUpdateCommands;

public class PaymentMethodUpdateCommandRequest:IRequest<PaymentMethodUpdateCommandResponse>
{
    public int Id { get; set; }
    public string PaymentMethodName { get; set; }
    public bool IsDeactive { get; set; }
}
