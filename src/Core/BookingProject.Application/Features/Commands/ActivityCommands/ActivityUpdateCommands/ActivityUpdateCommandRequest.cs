using MediatR;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivityUpdateCommands;

public class ActivityUpdateCommandRequest:IRequest<ActivityUpdateCommandResponse>
{
    public int Id { get; set; }
    public string ActivityName { get; set; }
    public bool IsDeactive { get; set; }
}
