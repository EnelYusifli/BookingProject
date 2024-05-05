using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageCreateCommands;
using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageDeleteCommands;
using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageSoftDeleteCommands;
using BookingProject.Application.Features.Commands.StaffLanguageCommands.StaffLanguageUpdateCommands;
using BookingProject.Application.Features.Queries.StaffLanguageQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StaffLanguagesController : ControllerBase
{
    private readonly IMediator _mediator;
    public StaffLanguagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        StaffLanguageGetAllQueryRequest request = new();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(StaffLanguageCreateCommandRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(StaffLanguageUpdateCommandRequest request, int id)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        StaffLanguageDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        StaffLanguageSoftDeleteCommandRequest request = new()
        {
            Id = id
        };
        return Ok(await _mediator.Send(request));
    }
}

