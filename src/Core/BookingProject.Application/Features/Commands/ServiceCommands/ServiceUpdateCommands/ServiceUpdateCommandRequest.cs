using MediatR;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceUpdateCommands;

public class ServiceUpdateCommandRequest:IRequest<ServiceUpdateCommandResponse>
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public bool IsDeactive { get; set; }
}
