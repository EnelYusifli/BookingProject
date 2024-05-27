using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.DiscountCommands.DiscountSoftDeleteCommands;
using BookingProject.Application.Features.Commands.OfferCommands.OfferCreateCommands;
using BookingProject.Application.Features.Commands.OfferCommands.OfferSoftDeleteCommands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class OffersController : ControllerBase
{
	private readonly IMediator _mediator;
	public OffersController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpPost]
	public async Task<IActionResult> Create(OfferCreateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> SoftDelete(int id)
	{
		OfferSoftDeleteCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
}

