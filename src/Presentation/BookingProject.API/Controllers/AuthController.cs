using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
//[Authorize(Roles ="Admin")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> CreateActivity(ActivityCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
}
