using BookingProject.Application.Features.Commands.ReviewCommands.ReviewCreateCommands;
using BookingProject.Application.Features.Commands.ReviewCommands.ReviewDeleteCommands;
using BookingProject.Application.Features.Commands.ReviewCommands.ReviewReportCommands;
using BookingProject.Application.Features.Commands.ReviewCommands.ReviewUpdateCommands;
using BookingProject.Application.Features.Commands.RoomCommands.RoomUpdateCommands;
using BookingProject.Application.Features.Queries.ReviewQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ReviewsController : ControllerBase
{
	private readonly IMediator _mediator;
	public ReviewsController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpGet]
	public async Task<IActionResult> GetAll(int? hotelId)
	{
		ReviewGetAllQueryRequest request = new()
		{
			HotelId = hotelId
		};
		return Ok(await _mediator.Send(request));
	}
    [HttpGet]
    public async Task<IActionResult> GetAllByOwner()
    {
		ReviewGetAllByOwnerQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
	public async Task<IActionResult> Create(ReviewCreateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> Update(ReviewUpdateCommandRequest request, int id)
	{
		request.Id = id;
		return Ok(await _mediator.Send(request));
	}
    [HttpPut("{id}")]
    public async Task<IActionResult> Report(int id)
    {
        ReviewReportCommandRequest request = new()
		{
			Id=id
		};
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		ReviewDeleteCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
}
