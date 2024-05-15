using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

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
		var user = await _userManager.GetUserAsync(request.User);

		return new GetUserQueryResponse { User = user };
	}
}
