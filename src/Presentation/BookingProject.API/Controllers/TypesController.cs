using BookingProject.Application.Features.Commands.TypeCommands.TypeCreateCommands;
using BookingProject.Application.Features.Commands.TypeCommands.TypeDeleteCommands;
using BookingProject.Application.Features.Commands.TypeCommands.TypeSoftDeleteCommands;
using BookingProject.Application.Features.Commands.TypeCommands.TypeUpdateCommands;
using BookingProject.Application.Features.Queries.TypeQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TypesController : ControllerBase
{
    private readonly IMediator _mediator;
    public TypesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        TypeGetAllQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(TypeCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(TypeUpdateCommandRequest request, int id)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        TypeDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		TypeGetByIdQueryRequest request = new() { Id = id };
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        TypeSoftDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
}
