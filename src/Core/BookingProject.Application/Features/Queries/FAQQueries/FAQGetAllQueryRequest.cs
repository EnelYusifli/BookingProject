using MediatR;

namespace BookingProject.Application.Features.Queries.FAQQueries;

public class FAQGetAllQueryRequest:IRequest<ICollection<FAQGetAllQueryResponse>>
{
}
