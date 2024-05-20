using BookingProject.Application.CustomExceptions;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;

namespace BookingProject.Application.Features.Queries.UserQueries;

public class GetUserQueryHandler : IRequestHandler<GetUserQueryRequest, GetUserQueryResponse>
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public GetUserQueryHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
	{
		_userManager = userManager;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<GetUserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
	{
		AppUser appUser = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			appUser = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
		}
		if (appUser is null)
			throw new NotFoundException("User not found");

		return new GetUserQueryResponse { User = appUser };
	}
}
