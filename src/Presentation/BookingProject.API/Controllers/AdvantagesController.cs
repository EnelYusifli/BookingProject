using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageCreateCommands;
using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageDeleteCommands;
using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageSoftDeleteCommands;
using BookingProject.Application.Features.Commands.AdvantageCommands.AdvantageUpdateCommands;
using BookingProject.Application.Features.Queries.AdvantageQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AdvantagesController : ControllerBase
{
    private readonly IMediator _mediator;
    public AdvantagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{hotelId}")]
    public async Task<IActionResult> GetAll(int hotelId)
    {
        AdvantageGetAllQueryRequest request = new()
        {
            HotelId=hotelId
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(AdvantageCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(AdvantageUpdateCommandRequest request, int id)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        AdvantageDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        AdvantageSoftDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
}