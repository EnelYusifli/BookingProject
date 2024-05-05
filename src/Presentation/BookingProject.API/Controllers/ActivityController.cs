using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityDeleteCommands;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivitySoftDeleteCommands;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityUpdateCommands;
using BookingProject.Application.Features.Queries.ActivityQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
//[Authorize(Roles ="Admin")]
public class ActivityController : ControllerBase
{
    private readonly IMediator _mediator;
    public ActivityController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        ActivityGetAllQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(ActivityCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut]
    public async Task<IActionResult> Update(ActivityUpdateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(ActivityDeleteCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut]
    public async Task<IActionResult> SoftDelete(ActivitySoftDeleteCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
}
