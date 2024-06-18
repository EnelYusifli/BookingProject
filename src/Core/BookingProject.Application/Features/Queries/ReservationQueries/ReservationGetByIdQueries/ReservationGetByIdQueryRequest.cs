using MediatR;

namespace BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetByIdQueries;

public class ReservationGetByIdQueryRequest:IRequest<ReservationGetByIdQueryResponse>
{
	public int Id { get; set; }
}
