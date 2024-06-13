using BookingProject.Application.Features.Queries.ReviewQueries;
using BookingProject.Application.Features.Queries.RoomQueries;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetAllByUserQueryResponse
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Address { get; set; }
	public string City { get; set; }
	public bool IsDeactive { get; set; }
	public string AppUserId { get; set; }
	public int ViewerCount { get; set; }
	public decimal StarPoint { get; set; }
	public string TypeName { get; set; }
	public string CountryName { get; set; }
	public List<string> ImageFileUrls { get; set; }
	public List<RoomGetByIdResponse> Rooms { get; set; }
}
