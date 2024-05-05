using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;

public class ActivityCreateCommandRequest:IRequest<ActivityCreateCommandResponse>
{
    [DataType(DataType.Text)]
    public string ActivityName { get; set; }
    public bool IsDeactive { get; set; } = false;
}
