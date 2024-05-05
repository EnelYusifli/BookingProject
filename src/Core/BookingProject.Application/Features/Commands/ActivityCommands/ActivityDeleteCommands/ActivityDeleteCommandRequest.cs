using MediatR;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivityDeleteCommands;

public class ActivityDeleteCommandRequest:IRequest<ActivityDeleteCommandResponse>
{
    public required int Id { get; set; }
}
