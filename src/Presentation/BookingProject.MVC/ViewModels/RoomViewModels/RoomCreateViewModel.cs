namespace BookingProject.MVC.ViewModels.RoomViewModels;

public class RoomCreateViewModel
{
	public string RoomName { get; set; }
	public int? HotelId { get; set; }
	public int AdultCount { get; set; }
	public int ChildCount { get; set; }
	public decimal ServiceFee { get; set; }
	public decimal PricePerNight { get; set; }
	public decimal Area { get; set; }
	public bool IsCancellable { get; set; }
	public int? CancelAfterDay { get; set; } = 0;
	public List<IFormFile> ImageFiles { get; set; }
}
