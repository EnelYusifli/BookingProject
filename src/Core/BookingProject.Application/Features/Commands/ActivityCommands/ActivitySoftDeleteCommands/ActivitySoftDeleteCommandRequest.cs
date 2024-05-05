using MediatR;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivitySoftDeleteCommands;

public class ActivitySoftDeleteCommandRequest:IRequest<ActivitySoftDeleteCommandResponse>
{
    public required int Id { get; set; }
}
