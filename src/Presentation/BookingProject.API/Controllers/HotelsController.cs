using BookingProject.Application.Features.Commands.HotelCommands.HotelApproveCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelDeleteCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelRefuseCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelSoftDeleteCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelUpdateCommands;
using BookingProject.Application.Features.Commands.WishlistCommands.WishlistAddCommands;
using BookingProject.Application.Features.Commands.WishlistCommands.WishlistRemoveCommands;
using BookingProject.Application.Features.Queries.HotelQueries;
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
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        HotelGetAllQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		HotelGetByIdQueryRequest request = new() { Id=id };
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> Update(HotelUpdateCommandRequest request, int id)
	{
		request.Id = id;
		return Ok(await _mediator.Send(request));
	}
	[HttpPost]
    public async Task<IActionResult> Create(HotelCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
	[HttpPost]
	public async Task<IActionResult> AddToWishlist(WishlistAddCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpDelete("{id}")]
	public async Task<IActionResult> RemoveFromWishlist(int id)
	{
		WishlistRemoveCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
	[HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        HotelDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        HotelSoftDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
	[HttpPut("{id}")]
	public async Task<IActionResult> ApproveHotel(int id)
	{
		HotelApproveCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> RefuseHotel(int id)
	{
		HotelRefuseCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
}
