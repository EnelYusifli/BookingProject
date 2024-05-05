using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;

public class ActivityCreateCommandResponse
{
    [DataType(DataType.Text)]
    public string Text { get; set; } = "Activity Created Successfully";
}
