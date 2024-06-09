using BookingProject.Application.Features.Commands.AboutCommands;
using BookingProject.Application.Features.Commands.CountryCommands.CountryUpdateCommands;
using BookingProject.Application.Features.Queries.AboutQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AboutController : ControllerBase
{
	private readonly IMediator _mediator;
	public AboutController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpPut]
	public async Task<IActionResult> Update(AboutUpdateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
    [HttpGet]
    public async Task<IActionResult> GetById()
    {
        AboutGetByIdQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
}
