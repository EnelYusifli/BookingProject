using BookingProject.Application.Features.Commands.ActivityCommands.ActivityDeleteCommands;
using BookingProject.Application.Features.Commands.FAQCommands.FAQCreateCommands;
using BookingProject.Application.Features.Commands.FAQCommands.FAQDeleteCommands;
using BookingProject.Application.Features.Commands.FAQCommands.FAQUpdateCommands;
using BookingProject.Application.Features.Queries.FAQQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class FAQsController : ControllerBase
{
    private readonly IMediator _mediator;
    public FAQsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        FAQGetAllQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(FAQCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(FAQUpdateCommandRequest request, int id)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        FAQDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		FAQGetByIdQueryRequest request = new() { Id = id };
		return Ok(await _mediator.Send(request));
	}
}
