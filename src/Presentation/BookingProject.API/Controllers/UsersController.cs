using BookingProject.Application.Features.Commands.UserCardCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IMediator _mediator;
	public UsersController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpPost]
	public async Task<IActionResult> AddUserCard(CardCreateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
}
