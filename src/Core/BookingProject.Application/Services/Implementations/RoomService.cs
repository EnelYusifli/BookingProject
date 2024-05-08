using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.RoomCommands.RoomCreateCommands;
using BookingProject.Application.Features.DTOs;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Services.Implementations;

public class RoomService:IRoomService
{
	private readonly IRoomRepository _repository;
	private readonly IMapper _mapper;
	private readonly IRoomImageRepository _roomImageRepository;
	private readonly IHotelRepository _hotelRepository;
	private readonly IConfiguration _configuration;

	public RoomService(IRoomRepository repository
		, IMapper mapper
		, IRoomImageRepository roomImageRepository
		, IHotelRepository hotelRepository
		, IConfiguration configuration)
	{
		_repository = repository;
		_mapper = mapper;
		_roomImageRepository = roomImageRepository;
		_hotelRepository = hotelRepository;
		_configuration = configuration;
	}
	public async Task<Room> CreateAsync(RoomCreateDto request,Hotel hotel)
	{
		if (request is null)
			throw new NotFoundException("Request not found");

		if (request.RoomName.IsNullOrEmpty())
			throw new BadRequestException("Name cannot be null");

		if (await _repository.Table.AnyAsync(x => x.RoomName.ToLower() == request.RoomName.ToLower()))
			throw new BadRequestException("Room Name is already exist");
		if (request.IsCancellable is false)
			request.CancelAfterDay = 0;
		var room = _mapper.Map<Room>(request);
		room.IsReserved = false;
		SaveFileExtension.Initialize(_configuration);
		foreach (var image in request.ImageFiles)
		{
			if (image is null)
				throw new NotFoundException($"Image not found");
			string url = await SaveFileExtension.SaveFile(image, "rooms");
			RoomImage roomImg = new()
			{
				Room = room,
				Url = url,
				IsDeactive = true
			};
			await _roomImageRepository.CreateAsync(roomImg);
		}
		room.Hotel = hotel;
		room.IsDeactive = true;
		await _repository.CreateAsync(room);
		return room;
	}
}
