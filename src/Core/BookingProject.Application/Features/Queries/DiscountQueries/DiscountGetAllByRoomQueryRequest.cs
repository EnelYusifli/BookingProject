using MediatR;

namespace BookingProject.Application.Features.Queries.DiscountQueries;

public class DiscountGetAllByRoomQueryRequest:IRequest<ICollection<DiscountGetAllByRoomQueryResponse>>
{
	public int RoomId {  get; set; }
}
