using BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelDeleteCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands;
using BookingProject.Application.Features.Commands.RoomCommands.RoomCreateCommands;
using BookingProject.Application.Features.Commands.RoomCommands.RoomDeleteCommands;
using BookingProject.Application.Features.Commands.RoomCommands.RoomSoftDeleteCommands;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Application.Features.Queries.RoomQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    public RoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{hotelId}")]
    public async Task<IActionResult> GetAll(int hotelId)
    {
        RoomGetAllQueryRequest request = new()
        {
            HotelId = hotelId
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(RoomCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        RoomDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        RoomSoftDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
}
