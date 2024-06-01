using MediatR;

namespace BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetAllByUserQueries;

public class ReservationGetAllByUserQueryRequest:IRequest<ICollection<ReservationGetAllByUserQueryResponse>>
{
    public string Id { get; set; }
}
