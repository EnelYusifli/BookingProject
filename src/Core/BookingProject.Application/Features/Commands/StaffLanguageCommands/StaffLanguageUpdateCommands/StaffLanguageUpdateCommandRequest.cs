using MediatR;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageUpdateCommands;

public class StaffLanguageUpdateCommandRequest:IRequest<StaffLanguageUpdateCommandResponse>
{
    public int Id { get; set; }
    public string StaffLanguageName { get; set; }
    public bool IsDeactive { get; set; }
}
