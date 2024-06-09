using BookingProject.Application.Features.Commands.TermsOfServiceCommands;
using BookingProject.Application.Features.Commands.CountryCommands.CountryUpdateCommands;
using BookingProject.Application.Features.Queries.TermsOfServiceQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TermsOfServiceController : ControllerBase
{
	private readonly IMediator _mediator;
	public TermsOfServiceController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpPut]
	public async Task<IActionResult> Update(TermsOfServiceUpdateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpGet]
	public async Task<IActionResult> GetById()
	{
		TermsOfServiceGetByIdQueryRequest request = new();
		return Ok(await _mediator.Send(request));
	}
}
