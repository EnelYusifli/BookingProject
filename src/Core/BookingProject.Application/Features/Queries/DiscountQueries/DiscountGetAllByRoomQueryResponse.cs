using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Queries.DiscountQueries;

public class DiscountGetAllByRoomQueryResponse
{
	public int Percent { get; set; }
	public bool IsDeactive { get; set; }
	public int Id { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
}
