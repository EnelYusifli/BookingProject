using MediatR;

namespace BookingProject.Application.Features.Queries.RoomQueries;

public class RoomGetByIdRequest:IRequest<RoomGetByIdResponse>
{
    public required int Id { get; set; }
}
