using BookingProject.Application.Features.Commands.UserCardCommands;
using BookingProject.Application.Features.Commands.UserCardCommands.UserCardDeleteCommands;
using BookingProject.Application.Features.Commands.WishlistCommands.WishlistAddCommands;
using BookingProject.Application.Features.Commands.WishlistCommands.WishlistRemoveCommands;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Application.Features.Queries.UserQueries;
using BookingProject.Application.Features.Queries.WishlistQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IMediator _mediator;
	public UsersController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpPost]
	public async Task<IActionResult> AddUserCard(CardCreateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(string id)
	{
		GetUserByIdQueryRequest request = new() { Id = id };
		return Ok(await _mediator.Send(request));
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> DeleteUserCard(int id)
	{
		UserCardDeleteCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		GetAllUsersQueryRequest request = new();
		return Ok(await _mediator.Send(request));
	}
	[HttpGet]
	public async Task<IActionResult> WishlistGetAll()
	{
		WishlistGetAllQueryRequest request = new();
		return Ok(await _mediator.Send(request));
	}
	[HttpPost("{hotelid}")]
	public async Task<IActionResult> AddToWishlist(int hotelid)
	{
		WishlistAddCommandRequest request = new() { HotelId=hotelid };
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
}
