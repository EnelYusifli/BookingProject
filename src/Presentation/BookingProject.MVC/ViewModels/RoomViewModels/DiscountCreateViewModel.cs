namespace BookingProject.MVC.ViewModels.RoomViewModels;

public class DiscountCreateViewModel
{
	public int RoomId { get; set; }
	public int Percent { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
}
