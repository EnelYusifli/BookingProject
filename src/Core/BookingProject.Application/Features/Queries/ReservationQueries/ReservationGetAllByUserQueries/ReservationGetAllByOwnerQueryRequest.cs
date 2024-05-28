using MediatR;

namespace BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;

public class ReservationGetAllByOwnerQueryRequest:IRequest<ICollection<ReservationGetAllByOwnerQueryResponse>>
{
}
