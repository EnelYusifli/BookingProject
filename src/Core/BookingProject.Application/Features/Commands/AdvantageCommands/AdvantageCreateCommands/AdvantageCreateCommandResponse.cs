using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageCreateCommands;

public class AdvantageCreateCommandResponse
{
    [DataType(DataType.Text)]
    public string Text { get; set; } = "Advantage Created Successfully";
}
