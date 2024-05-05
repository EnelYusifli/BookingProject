using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceCreateCommands;

public class ServiceCreateCommandRequest:IRequest<ServiceCreateCommandResponse>
{
    [DataType(DataType.Text)]
    public string ServiceName { get; set; }
    public bool IsDeactive { get; set; } = false;
}
