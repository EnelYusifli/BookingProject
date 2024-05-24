using AutoMapper;
using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.UserQueries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, ICollection<GetAllUsersQueryResponse>>
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IMapper _mapper;

	public GetAllUsersQueryHandler(UserManager<AppUser> userManager,IMapper mapper)
    {
		_userManager = userManager;
		_mapper = mapper;
	}
	public async Task<ICollection<GetAllUsersQueryResponse>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
	{
		var users = await _userManager.Users.Include(u => u.Hotels).ToListAsync(cancellationToken);

		if (users == null || !users.Any())
			throw new Exception("User not found");

		var dtos = new List<GetAllUsersQueryResponse>();

		foreach (var user in users)
		{
			var roles = await _userManager.GetRolesAsync(user);

			var userDto = new GetAllUsersQueryResponse
			{
				FirstName = user.FirstName,
				Id = user.Id,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				HotelCount = user.Hotels.Count(),
				UserName = user.UserName,
				Birthdate = user.Birthdate,
				RecoveryEmail = user.RecoveryEmail,
				ProfilePhotoUrl = user.ProfilePhotoUrl,
				Roles = roles
			};

			dtos.Add(userDto);
		}

		return dtos;
	}

}
