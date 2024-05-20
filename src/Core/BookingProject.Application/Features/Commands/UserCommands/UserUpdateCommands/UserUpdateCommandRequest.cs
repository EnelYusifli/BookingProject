using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Commands.UserCommands.UserUpdateCommands;

public class UserUpdateCommandRequest:IRequest<UserUpdateCommandResponse>
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
	public string UserName { get; set; }
	public string? PhoneNumber { get; set; }
    public DateOnly? Birthdate { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
}
