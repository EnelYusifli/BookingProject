using MediatR;

namespace BookingProject.Application.Features.Queries.AdvantageQueries;

public class AdvantageGetAllQueryRequest:IRequest<ICollection<AdvantageGetAllQueryResponse>>
{
}
