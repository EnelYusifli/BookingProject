using MediatR;
using System.Security.Claims;

namespace BookingProject.Application.Features.Queries.UserQueries;

public class GetUserQueryRequest : IRequest<GetUserQueryResponse>
{
	//public ClaimsPrincipal User { get; }

	//public GetUserQueryRequest(ClaimsPrincipal user)
	//{
	//	User = user;
	//}
}

