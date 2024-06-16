namespace BookingProject.MVC.ViewModels.RoomViewModels;

public class DiscountGetViewModel
{
	public int RoomId { get; set; }
	public bool IsDeactive { get; set; }
	public int Id { get; set; }
	public int Percent { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
}
