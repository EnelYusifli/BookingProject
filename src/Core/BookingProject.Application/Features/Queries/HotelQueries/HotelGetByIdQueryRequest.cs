using MediatR;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetByIdQueryRequest:IRequest<HotelGetByIdQueryResponse>
{
    public required int Id { get; set; }
}
