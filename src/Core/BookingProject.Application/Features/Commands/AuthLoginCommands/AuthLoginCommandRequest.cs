using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;

public class AuthLoginCommandRequest:IRequest<AuthLoginCommandResponse>
{
    [DataType(DataType.Text)]
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
