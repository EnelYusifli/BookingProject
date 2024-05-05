using MediatR;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageDeleteCommands;

public class StaffLanguageDeleteCommandRequest:IRequest<StaffLanguageDeleteCommandResponse>
{
    public required int Id { get; set; }
}
