using MediatR;

namespace BookingProject.Application.Features.Commands.ServiceCommands.ServiceDeleteCommands;

public class ServiceDeleteCommandRequest : IRequest<ServiceDeleteCommandResponse>
{
    public required int Id { get; set; }
}
