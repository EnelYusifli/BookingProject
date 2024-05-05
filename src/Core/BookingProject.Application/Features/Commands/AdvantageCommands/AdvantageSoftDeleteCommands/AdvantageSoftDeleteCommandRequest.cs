using MediatR;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageSoftDeleteCommands;

public class AdvantageSoftDeleteCommandRequest:IRequest<AdvantageSoftDeleteCommandResponse>
{
    public required int Id { get; set; }
}
