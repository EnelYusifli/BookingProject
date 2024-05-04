using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;

public class AuthLoginCommandResponse
{
    [DataType(DataType.Text)]
    public string UserName { get; set; }
    public string Token { get; set; }
}
