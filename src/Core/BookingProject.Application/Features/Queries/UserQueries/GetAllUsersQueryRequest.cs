using MediatR;

namespace BookingProject.Application.Features.Queries.UserQueries;

public class GetAllUsersQueryRequest:IRequest<ICollection<GetAllUsersQueryResponse>>
{
}
