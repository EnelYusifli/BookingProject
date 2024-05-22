using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.ReservationCommands.ReservationCreateCommands;

public class ReservationCreateCommandHandler : IRequestHandler<ReservationCreateCommandRequest, ReservationCreateCommandResponse>
{
	private readonly IReservationRepository _reservationRepository;
	private readonly IRoomRepository _roomRepository;
	private readonly UserManager<AppUser> _userManager;
	private readonly IMapper _mapper;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ReservationCreateCommandHandler(IReservationRepository reservationRepository,
		IRoomRepository roomRepository,
		UserManager<AppUser> userManager,
		IMapper mapper,
		IHttpContextAccessor httpContextAccessor)
	{
		_reservationRepository = reservationRepository;
		_roomRepository = roomRepository;
		_userManager = userManager;
		_mapper = mapper;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<ReservationCreateCommandResponse> Handle(ReservationCreateCommandRequest request, CancellationToken cancellationToken)
	{
		var room = await _roomRepository.GetByIdAsync(request.RoomId);
		if (room is null || room.IsDeactive)
		{
			throw new ArgumentException("Room not found or inactive.");
		}
		AppUser user = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
		}
		if (user is null)
			throw new NotFoundException("User not found");


		var existingReservation = await _reservationRepository.Table
			.Where(r => r.RoomId == request.RoomId && r.IsCancelled==false && r.IsDeactive==false &&
						((r.StartTime <= request.EndTime && r.EndTime >= request.StartTime) ||
						 (r.StartTime >= request.StartTime && r.EndTime <= request.EndTime)))
			.FirstOrDefaultAsync();

		if (existingReservation is not null)
			throw new ArgumentException("Room already reserved for the specified time.");
		Reservation reservation=_mapper.Map<Reservation>(request);
		reservation.IsCancelled = false;
		reservation.AppUser = user;
		reservation.AppUserId = user.Id;
		room.IsReserved = true;

		await _reservationRepository.CreateAsync(reservation);
		await _reservationRepository.CommitAsync();
		return new ReservationCreateCommandResponse();
	}
}
