﻿using BookingProject.Application.Features.Commands.ActivityCommands.ActivityCreateCommands;
using BookingProject.Application.Features.Commands.MessageCommands.MessageCreateCommands;
using BookingProject.Application.Features.Queries.CountryQueries;
using BookingProject.Application.Features.Queries.MessageQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class MessagesController : ControllerBase
{
	private readonly IMediator _mediator;
	public MessagesController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpPost]
	public async Task<IActionResult> Create(MessageCreateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		MessageGetAllQueryRequest request = new();
		return Ok(await _mediator.Send(request));
	}
}