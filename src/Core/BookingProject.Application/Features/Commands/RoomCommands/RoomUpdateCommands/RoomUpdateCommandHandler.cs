using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.RoomCommands.RoomUpdateCommands;

public class RoomUpdateCommandHandler : IRequestHandler<RoomUpdateCommandRequest, RoomUpdateCommandResponse>
{
	private readonly IRoomRepository _repository;
	private readonly IMapper _mapper;
	private readonly IRoomImageRepository _roomImageRepository;
	private readonly IConfiguration _configuration;

	public RoomUpdateCommandHandler(IRoomRepository repository,
		IMapper mapper,
		IRoomImageRepository roomImageRepository,
		IConfiguration configuration)
	{
		_repository = repository;
		_mapper = mapper;
		_roomImageRepository = roomImageRepository;
		_configuration = configuration;
	}

	public async Task<RoomUpdateCommandResponse> Handle(RoomUpdateCommandRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			throw new NotFoundException("Request not found");

		if (request.RoomName.IsNullOrEmpty())
			throw new BadRequestException("Name cannot be null");
		Room room = await _repository.Table.Include(x=>x.RoomImages).FirstOrDefaultAsync(x=>x.Id==request.Id);
		if (room is null)
			throw new NotFoundException($"Room with ID {request.Id} not found");
		Room existRoom = await _repository.Table.FirstOrDefaultAsync(x => x.RoomName == request.RoomName && x.HotelId==room.HotelId);
		if (existRoom is not null && existRoom.RoomName.ToLower() != room.RoomName.ToLower())
			throw new BadRequestException("Room name already exist");
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
				IsDeactive = request.IsDeactive
			};
			await _roomImageRepository.CreateAsync(roomImg);
		}

		foreach (var id in request.DeletedImageFileIds)
		{
			RoomImage img = await _roomImageRepository.GetByIdAsync(id);
			if (img is null || img.RoomId!=room.Id)
				throw new NotFoundException($"Image with ID {id} not found in room");

			_roomImageRepository.Delete(img);
			await SaveFileExtension.DeleteFileAsync(img.Url);
		}
		room.ModifiedDate=DateTime.Now;
		_mapper.Map(request, room);
		await _repository.CommitAsync();
		return new RoomUpdateCommandResponse();
	}
}

