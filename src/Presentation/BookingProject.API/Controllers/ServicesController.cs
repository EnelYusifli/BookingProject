using BookingProject.Application.Features.Commands.ServiceCommands.ServiceCreateCommands;
using BookingProject.Application.Features.Commands.ServiceCommands.ServiceDeleteCommands;
using BookingProject.Application.Features.Commands.ServiceCommands.ServiceSoftDeleteCommands;
using BookingProject.Application.Features.Commands.ServiceCommands.ServiceUpdateCommands;
using BookingProject.Application.Features.Queries.ServiceQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        ServiceGetAllQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(ServiceCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(ServiceUpdateCommandRequest request, int id)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ServiceDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        ServiceSoftDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
}
