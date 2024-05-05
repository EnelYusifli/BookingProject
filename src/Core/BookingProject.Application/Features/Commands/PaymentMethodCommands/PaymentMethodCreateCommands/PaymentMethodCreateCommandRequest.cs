using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodCreateCommands;

public class PaymentMethodCreateCommandRequest:IRequest<PaymentMethodCreateCommandResponse>
{
    [DataType(DataType.Text)]
    public string PaymentMethodName { get; set; }
    public bool IsDeactive { get; set; } = false;
}
