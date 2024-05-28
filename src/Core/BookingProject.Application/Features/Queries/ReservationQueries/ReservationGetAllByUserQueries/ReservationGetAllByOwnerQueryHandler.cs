using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;

public class ReservationGetAllByOwnerQueryHandler : IRequestHandler<ReservationGetAllByOwnerQueryRequest, ICollection<ReservationGetAllByOwnerQueryResponse>>
{
	private readonly IMapper _mapper;
	private readonly IReservationRepository _reservationRepository;
	private readonly UserManager<AppUser> _userManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ReservationGetAllByOwnerQueryHandler(IMapper mapper, IReservationRepository reservationRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
	{
		_mapper = mapper;
		_reservationRepository = reservationRepository;
		_userManager = userManager;
		_httpContextAccessor = httpContextAccessor;
	}
	public async Task<ICollection<ReservationGetAllByOwnerQueryResponse>> Handle(ReservationGetAllByOwnerQueryRequest request, CancellationToken cancellationToken)
	{
		AppUser user = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
		}
		if (user is null)
			throw new NotFoundException("User not found");
		ICollection<Reservation> act = await _reservationRepository.Table.Include(x => x.Room).ThenInclude(x => x.Hotel).Where(r => r.Room.Hotel.AppUserId == user.Id).ToListAsync();
		if (act is null) throw new Exception("Reservation not found");
		ICollection<ReservationGetAllByOwnerQueryResponse> dtos = _mapper.Map<ICollection<ReservationGetAllByOwnerQueryResponse>>(act);

		return dtos;
	}
	
}
