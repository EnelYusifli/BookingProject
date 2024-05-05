using BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands;
using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;
using BookingProject.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> ForgetPassword(ForgotPasswordQueryRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
}
