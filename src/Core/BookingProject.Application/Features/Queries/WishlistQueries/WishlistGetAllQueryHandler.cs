using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Queries.ReviewQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.WishlistQueries;

public class WishlistGetAllQueryHandler : IRequestHandler<WishlistGetAllQueryRequest, ICollection<WishlistGetAllQueryResponse>>
{
	private readonly IWishlistRepository _repository;
	private readonly IMapper _mapper;
	private readonly UserManager<AppUser> _userManager;

	public WishlistGetAllQueryHandler(IWishlistRepository repository, IMapper mapper,UserManager<AppUser> userManager)
    {
		_repository = repository;
		_mapper = mapper;
		_userManager = userManager;
	}
    public async Task<ICollection<WishlistGetAllQueryResponse>> Handle(WishlistGetAllQueryRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			throw new NotFoundException("Request not found");
		AppUser user=await _userManager.FindByIdAsync(request.UserId);
		if (user is null)
			throw new NotFoundException("User Not Found");
		ICollection<UserWishlistHotel> act = await _repository.Table
		   .Where(x => x.UserId == request.UserId && x.Hotel.IsDeactive == false && x.IsDeactive == false)
		   .ToListAsync();
		if (act is null) throw new Exception("Review not found");
		ICollection<WishlistGetAllQueryResponse> dtos = _mapper.Map<ICollection<WishlistGetAllQueryResponse>>(act);
		return dtos;
	}
}
