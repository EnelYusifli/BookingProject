using MediatR;

namespace BookingProject.Application.Features.Queries.ReviewQueries;

public class ReviewGetAllByOwnerQueryRequest:IRequest<ICollection<ReviewGetAllQueryResponse>>
{
    public string Id { get; set; }
}
