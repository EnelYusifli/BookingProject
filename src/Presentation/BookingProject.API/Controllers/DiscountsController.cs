using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.CountryCommands.CountrySoftDeleteCommands;
using BookingProject.Application.Features.Commands.DiscountCommands.DiscountCreateCommands;
using BookingProject.Application.Features.Commands.DiscountCommands.DiscountSoftDeleteCommands;
using BookingProject.Application.Features.Queries.AdvantageQueries;
using BookingProject.Application.Features.Queries.DiscountQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DiscountsController : ControllerBase
{
	private readonly IMediator _mediator;

	public DiscountsController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpPost("{roomid}")]
	public async Task<IActionResult> Create(DiscountCreateCommandRequest request,int roomid)
	{
		request.RoomId = roomid;
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> SoftDelete(int id)
	{
		DiscountSoftDeleteCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
	[HttpGet("{roomId}")]
	public async Task<IActionResult> GetAll(int roomId)
	{
		DiscountGetAllByRoomQueryRequest request = new()
		{
			RoomId = roomId
		};
		return Ok(await _mediator.Send(request));
	}
}
