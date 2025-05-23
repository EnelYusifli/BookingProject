﻿using BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands;
using BookingProject.Application.Features.Commands.HotelCommands.HotelUpdateCommands;
using BookingProject.Application.Features.Commands.ReservationCommands.ReservationCancelCommands;
using BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;
using BookingProject.Application.Features.Queries.CountryQueries;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;
using BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetByIdQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ReservationsController : ControllerBase
{
	private readonly IMediator _mediator;
	public ReservationsController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost]
	public async Task<IActionResult> Create(ReservationCreateCommandRequest request)
	{
		return Ok(await _mediator.Send(request));
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> Cancel(int id)
	{
		ReservationCancelCommandRequest request = new()
		{
			ReservationId = id
		};
		return Ok(await _mediator.Send(request));
	}
	[HttpGet("{id}")]
	public async Task<IActionResult> GetAllByUser(string id)
	{
		ReservationGetAllByUserQueryRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
	[HttpGet("{id}")]
	public async Task<IActionResult> GetAllByOwner(string id)
	{
		ReservationGetAllByOwnerQueryRequest request = new()
		{
			Id = id
		};
		return Ok(await _mediator.Send(request));
	}
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		ReservationGetByIdQueryRequest request = new() { Id = id };
		return Ok(await _mediator.Send(request));
	}
}
