using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodCreateCommands;

public class PaymentMethodCreateCommandResponse
{
    [DataType(DataType.Text)]
    public string Text { get; set; } = "PaymentMethod Created Successfully";
}
