using AutoMapper;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.UserQueries;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetUserByIdQueryHandler(UserManager<AppUser> userManager, IMapper mapper,IMediator mediator)
	{
		_userManager = userManager;
		_mapper = mapper;
		_mediator = mediator;
	}
		public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.Users.Include(u => u.Hotels).ThenInclude(x => x.HotelImages).Include(x=>x.Hotels).ThenInclude(x=>x.Country).FirstOrDefaultAsync(x=>x.Id==request.Id);

		if (user is null)
			throw new Exception("User not found");
		var dto=_mapper.Map<GetUserByIdQueryResponse>(user);
		return dto;
	}
}
