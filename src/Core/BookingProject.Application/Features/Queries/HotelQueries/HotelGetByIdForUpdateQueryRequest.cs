using MediatR;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetByIdForUpdateQueryRequest:IRequest<HotelGetByIdForUpdateQueryResponse>
{
	public int Id { get; set; }
}
