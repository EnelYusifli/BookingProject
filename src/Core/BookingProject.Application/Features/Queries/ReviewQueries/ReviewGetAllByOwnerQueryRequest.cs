using MediatR;

namespace BookingProject.Application.Features.Queries.ReviewQueries;

public class ReviewGetAllByOwnerQueryRequest:IRequest<ICollection<ReviewGetAllQueryResponse>>
{
}
