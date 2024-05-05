using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageCreateCommands;

public class StaffLanguageCreateCommandRequest:IRequest<StaffLanguageCreateCommandResponse>
{
    [DataType(DataType.Text)]
    public string StaffLanguageName { get; set; }
    public bool IsDeactive { get; set; } = false;
}
