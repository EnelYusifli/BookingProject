using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Queries.ReservationQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;

public class ReservationGetAllByUserQueryHandler : IRequestHandler<ReservationGetAllByUserQueryRequest, ICollection<ReservationGetAllByUserQueryResponse>>
{
	private readonly IMapper _mapper;
	private readonly IReservationRepository _reservationRepository;
	private readonly UserManager<AppUser> _userManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ReservationGetAllByUserQueryHandler(IMapper mapper,IReservationRepository reservationRepository,UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor)
    {
		_mapper = mapper;
		_reservationRepository = reservationRepository;
		_userManager = userManager;
		_httpContextAccessor = httpContextAccessor;
	}
    public async Task<ICollection<ReservationGetAllByUserQueryResponse>> Handle(ReservationGetAllByUserQueryRequest request, CancellationToken cancellationToken)
	{
		AppUser user = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
		}
		if (user is null)
			throw new NotFoundException("User not found");
		ICollection<Reservation> act = await _reservationRepository.Table.Include(x=>x.Room).ThenInclude(x=>x.Hotel).Where(x=>x.AppUserId==user.Id).ToListAsync();
		if (act is null) throw new Exception("Reservation not found");
		ICollection<ReservationGetAllByUserQueryResponse> dtos = _mapper.Map<ICollection<ReservationGetAllByUserQueryResponse>>(act);

		return dtos;
	}
}
