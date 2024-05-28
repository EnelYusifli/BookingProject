using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetAllByUserQueryHandler : IRequestHandler<HotelGetAllByUserQueryRequest, ICollection<HotelGetAllByUserQueryResponse>>
{
	private readonly IHotelRepository _repository;
	private readonly IMapper _mapper;
	private readonly UserManager<AppUser> _userManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public HotelGetAllByUserQueryHandler(IHotelRepository repository, IMapper mapper, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_mapper = mapper;
		_userManager = userManager;
		_httpContextAccessor = httpContextAccessor;
	}
	public async Task<ICollection<HotelGetAllByUserQueryResponse>> Handle(HotelGetAllByUserQueryRequest request, CancellationToken cancellationToken)
	{
		AppUser user = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
		}
		if (user is null)
			throw new NotFoundException("User not found");
		ICollection<Hotel> act = await _repository.Table
		   .Include(x => x.HotelImages)
		   .Include(x => x.Type)
		   .Include(x => x.Country)
		   .Include(x => x.Rooms)
		   .Where(x=>x.AppUserId==user.Id)
		   .ToListAsync();
		if (act is null) throw new Exception("Hotel not found");
		ICollection<HotelGetAllByUserQueryResponse> dtos = _mapper.Map<ICollection<HotelGetAllByUserQueryResponse>>(act);
		return dtos;
	}
}
