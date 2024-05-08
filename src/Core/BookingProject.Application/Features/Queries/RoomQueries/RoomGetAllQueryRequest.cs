using MediatR;

namespace BookingProject.Application.Features.Queries.RoomQueries;

public class RoomGetAllQueryRequest:IRequest<ICollection<RoomGetAllQueryResponse>>
{
    public int HotelId { get; set; }
}
