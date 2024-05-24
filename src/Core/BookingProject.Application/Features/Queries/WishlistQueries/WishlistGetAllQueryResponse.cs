using BookingProject.Application.Features.Queries.HotelQueries;

namespace BookingProject.Application.Features.Queries.WishlistQueries;

public class WishlistGetAllQueryResponse
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public HotelGetByIdQueryResponse Hotel { get; set; }
}
