using MediatR;

namespace BookingProject.Application.Features.Queries.WishlistQueries;

public class WishlistGetAllQueryRequest:IRequest<ICollection<WishlistGetAllQueryResponse>>
{
}
