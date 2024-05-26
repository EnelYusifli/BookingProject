using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityDeleteCommands;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivitySoftDeleteCommands;
using BookingProject.Application.Features.Commands.ActivityCommands.ActivityUpdateCommands;
using BookingProject.Application.Features.Queries.ActivityQueries;
using BookingProject.Application.Features.Queries.HotelQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ActivitiesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ActivitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        ActivityGetAllQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        ActivityGetByIdQueryRequest request = new() { Id = id };
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(ActivityCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(ActivityUpdateCommandRequest request,int id)
    {
        request.Id=id;
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ActivityDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        ActivitySoftDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
}
