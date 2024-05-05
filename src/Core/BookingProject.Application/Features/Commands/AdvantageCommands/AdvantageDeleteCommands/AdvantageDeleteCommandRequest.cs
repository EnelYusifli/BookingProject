using MediatR;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageDeleteCommands;

public class AdvantageDeleteCommandRequest:IRequest<AdvantageDeleteCommandResponse>
{
    public required int Id { get; set; }
}
