using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.RoomCommands.RoomCreateCommands;

public class RoomCreateCommandHandler : IRequestHandler<RoomCreateCommandRequest, RoomCreateCommandResponse>
{
    private readonly IRoomRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRoomImageRepository _roomImageRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IConfiguration _configuration;

    public RoomCreateCommandHandler(IRoomRepository repository
        ,IMapper mapper
        ,IRoomImageRepository roomImageRepository 
        ,IHotelRepository hotelRepository
        ,IConfiguration configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _roomImageRepository = roomImageRepository;
        _hotelRepository = hotelRepository;
        _configuration = configuration;
    }
    public async Task<RoomCreateCommandResponse> Handle(RoomCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new NotFoundException("Request not found");

        if (request.RoomName.IsNullOrEmpty())
            throw new BadRequestException("Name cannot be null");

        if (await _repository.Table.AnyAsync(x => x.RoomName.ToLower() == request.RoomName.ToLower()))
            throw new BadRequestException("Room Name is already exist");
        Hotel hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
        if (hotel is null || hotel.IsDeactive==true)
            throw new NotFoundException("Hotel not found");
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
                IsDeactive = request.IsDeactive
            };
            await _roomImageRepository.CreateAsync(roomImg);
        }
        await _repository.CreateAsync(room);
        await _repository.CommitAsync();
        return new RoomCreateCommandResponse();
    }
}
