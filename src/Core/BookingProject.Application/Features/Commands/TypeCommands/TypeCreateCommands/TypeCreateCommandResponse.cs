using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.TypeCommands.TypeCreateCommands;

public class TypeCreateCommandResponse
{
    [DataType(DataType.Text)]
    public string Text { get; set; } = "Type Created Successfully";
}
