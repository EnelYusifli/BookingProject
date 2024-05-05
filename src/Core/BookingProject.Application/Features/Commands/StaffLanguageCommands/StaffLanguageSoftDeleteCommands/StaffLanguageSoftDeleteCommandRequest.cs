using MediatR;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageSoftDeleteCommands;

public class StaffLanguageSoftDeleteCommandRequest:IRequest<StaffLanguageSoftDeleteCommandResponse>
{
    public required int Id { get; set; }
}
