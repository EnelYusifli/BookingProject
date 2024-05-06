using MediatR;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetAllQueryRequest:IRequest<ICollection<HotelGetAllQueryResponse>>
{
}
