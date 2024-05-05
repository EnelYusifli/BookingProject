using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceCreateCommands;

public class ServiceCreateCommandResponse
{
    [DataType(DataType.Text)]
    public string Text { get; set; } = "Service Created Successfully";
}
