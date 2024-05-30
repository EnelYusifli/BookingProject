using MediatR;

namespace BookingProject.Application.Features.Queries.ReviewQueries;

public class ReviewGetAllQueryRequest:IRequest<ICollection<ReviewGetAllQueryResponse>>
{
    public int? HotelId { get; set; }
}
