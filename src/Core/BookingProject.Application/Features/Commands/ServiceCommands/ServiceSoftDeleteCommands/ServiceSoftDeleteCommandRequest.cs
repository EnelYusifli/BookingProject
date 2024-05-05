using MediatR;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceSoftDeleteCommands;

public class ServiceSoftDeleteCommandRequest:IRequest<ServiceSoftDeleteCommandResponse>
{
    public required int Id { get; set; }
}
