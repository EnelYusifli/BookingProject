using BookingProject.Application.Features.DTOs;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Services.Interfaces;

public interface IRoomService
{
	Task<Room> CreateAsync(RoomCreateDto request, Hotel hotel);
}
