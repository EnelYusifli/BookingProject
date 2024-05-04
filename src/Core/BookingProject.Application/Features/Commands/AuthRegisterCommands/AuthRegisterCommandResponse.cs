using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;


public class AuthRegisterCommandResponse
{
    [DataType(DataType.Text)]
    public string Text { get; set; } = "Created as customer";
}
