using MediatR;

namespace BookingProject.Application.Features.Queries.ActivityQueries;

public class ActivityGetAllQueryRequest:IRequest<ICollection<ActivityGetAllQueryResponse>>
{
}
