using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodCreateCommands;
using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodDeleteCommands;
using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodSoftDeleteCommands;
using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodUpdateCommands;
using BookingProject.Application.Features.Queries.PaymentMethodQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PaymentMethodsController : ControllerBase
{
    private readonly IMediator _mediator;
    public PaymentMethodsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        PaymentMethodGetAllQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(PaymentMethodCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(PaymentMethodUpdateCommandRequest request, int id)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		PaymentMethodGetByIdQueryRequest request = new() { Id = id };
		return Ok(await _mediator.Send(request));
	}
	[HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        PaymentMethodDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        PaymentMethodSoftDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
}


