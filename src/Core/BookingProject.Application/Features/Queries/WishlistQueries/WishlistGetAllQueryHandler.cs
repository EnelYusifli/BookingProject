using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Queries.ReviewQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.WishlistQueries;

public class WishlistGetAllQueryHandler : IRequestHandler<WishlistGetAllQueryRequest, ICollection<WishlistGetAllQueryResponse>>
{
	private readonly IWishlistRepository _repository;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IMapper _mapper;
	private readonly UserManager<AppUser> _userManager;

	public WishlistGetAllQueryHandler(IWishlistRepository repository,IHttpContextAccessor httpContextAccessor, IMapper mapper,UserManager<AppUser> userManager)
    {
		_repository = repository;
		_httpContextAccessor = httpContextAccessor;
		_mapper = mapper;
		_userManager = userManager;
	}
    public async Task<ICollection<WishlistGetAllQueryResponse>> Handle(WishlistGetAllQueryRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			throw new NotFoundException("Request not found");
		AppUser user = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
		}
		if (user is null)
			throw new NotFoundException("User not found");
		ICollection<UserWishlistHotel> act = await _repository.Table
		   .Where(x => x.UserId == user.Id && x.Hotel.IsDeactive == false && x.IsDeactive == false)
		   .Include(x=>x.Hotel).ThenInclude(x=>x.Type)
		   .Include(x => x.Hotel).ThenInclude(x => x.HotelImages)
		   .Include(x => x.Hotel).ThenInclude(x => x.Rooms)
		   .ToListAsync();
		//if (act is null) throw new Exception("Item not found");
		ICollection<WishlistGetAllQueryResponse> dtos = _mapper.Map<ICollection<WishlistGetAllQueryResponse>>(act);
		return dtos;
	}
}
