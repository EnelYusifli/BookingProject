using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;
    public HotelsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Create(HotelCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
}
