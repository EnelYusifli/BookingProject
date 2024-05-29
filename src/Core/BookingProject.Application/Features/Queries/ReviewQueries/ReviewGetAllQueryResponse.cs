using Microsoft.AspNetCore.Http;

namespace BookingProject.Application.Features.Queries.ReviewQueries;

public class ReviewGetAllQueryResponse
{
	public int HotelId { get; set; }
	public DateTime CreatedDate { get; set; }
	public string UserId { get; set; }
	public int StarPoint { get; set; }
	public string ReviewMessage { get; set; }
	public List<string>? ReviewImageUrls { get; set; }
}
