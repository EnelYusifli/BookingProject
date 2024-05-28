using BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;
using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;
using BookingProject.Application.Features.Commands.ResetPasswordCommands;
using BookingProject.Application.Features.Commands.UserCommands.RefreshCommands;
using BookingProject.Application.Features.Commands.UserCommands.UserUpdateCommands;
using BookingProject.Application.Features.Commands.UserCommands.UserUpdatePasswordCommands;
using BookingProject.Application.Features.Queries;
using BookingProject.Application.Features.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
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
	[HttpPut]
	public async Task<IActionResult> ResetPassword(ResetPasswordCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpPut]
	public async Task<IActionResult> UpdateUser(UserUpdateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpGet]
    public async Task<IActionResult> GetAuthUser()
	{
		//ClaimsPrincipal user = HttpContext.User;
		GetUserQueryRequest request = new();
		return Ok(await _mediator.Send(request));
	}
    [HttpPut]
	public async Task<IActionResult> RefreshToken(RefreshCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpPut]
	public async Task<IActionResult> ChangePassword(UserUpdatePasswordCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	
}
