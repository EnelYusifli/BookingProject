using BookingProject.Application.Features.Commands.CountryCommands.CountryCreateCommands;
using BookingProject.Application.Features.Commands.CountryCommands.CountryDeleteCommands;
using BookingProject.Application.Features.Commands.CountryCommands.CountrySoftDeleteCommands;
using BookingProject.Application.Features.Commands.CountryCommands.CountryUpdateCommands;
using BookingProject.Application.Features.Queries.CountryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CountriesController : ControllerBase
{
	private readonly IMediator _mediator;
	public CountriesController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		CountryGetAllQueryRequest request = new();
		return Ok(await _mediator.Send(request));
	}
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		CountryGetByIdQueryRequest request = new() { Id = id };
		return Ok(await _mediator.Send(request));
	}
	[HttpPost]
	public async Task<IActionResult> Create(CountryCreateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> Update(CountryUpdateCommandRequest request, int id)
	{
		request.Id = id;
		return Ok(await _mediator.Send(request));
	}
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		CountryDeleteCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> SoftDelete(int id)
	{
		CountrySoftDeleteCommandRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
}
