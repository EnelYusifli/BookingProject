using BookingProject.Application.Features.Queries.HotelQueries;
using BookingProject.Application.Features.Queries.RoomQueries;
using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Queries.ReviewQueries;

public class ReviewGetAllQueryResponse
{
	public int HotelId { get; set; }
	public int Id { get; set; }
	public bool IsReported { get; set; }
	public bool IsDeactive { get; set; }
	public DateTime CreatedDate { get; set; }
	public string UserId { get; set; }
	public string UserName { get; set; }
	public string UserPpUrl { get; set; }
	public int StarPoint { get; set; }
	public string ReviewMessage { get; set; }
	public List<string>? ReviewImageUrls { get; set; }
	public string HotelName { get; set; }
}
