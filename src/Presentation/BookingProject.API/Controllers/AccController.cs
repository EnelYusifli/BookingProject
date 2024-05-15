using BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;
using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;
using BookingProject.Application.Features.Commands.ResetPasswordCommands;
using BookingProject.Application.Features.Queries;
using BookingProject.Application.Features.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccController : ControllerBase
{
    private readonly IMediator _mediator;
    public AccController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Register(AuthRegisterCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Login(AuthLoginCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordQueryRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
	[HttpPost]
	public async Task<IActionResult> ResetPassword(ResetPasswordCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
    [HttpGet]
    public async Task<IActionResult> GetAuthUser()
	{
		ClaimsPrincipal user = HttpContext.User;
		GetUserQueryRequest request = new(user);
		return Ok(await _mediator.Send(request));
	}
}
