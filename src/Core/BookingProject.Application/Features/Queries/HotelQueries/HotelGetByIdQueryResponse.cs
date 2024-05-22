using BookingProject.Application.Features.Queries.RoomQueries;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetByIdQueryResponse
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Desc { get; set; }
	public string Address { get; set; }
	public string CountryName { get; set; }
	public string City { get; set; }
	public bool IsDeactive { get; set; }
	public bool IsApproved { get; set; }
	public bool IsRefused { get; set; }
	public string AppUserId { get; set; }
	public int ViewerCount { get; set; }
	public decimal StarPoint { get; set; }
	public string TypeName { get; set; }
	public List<string> ActivityNames { get; set; }
	public List<string> ImageFileUrls { get; set; }
	public List<string> AdvantageNames { get; set; }
	public List<string> PaymentMethodNames { get; set; }
	public List<string> ServiceNames { get; set; }
	public List<string> StaffLanguageNames { get; set; }
	public List<RoomGetByIdResponse> Rooms { get; set; }
}
