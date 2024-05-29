namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ReviewGetViewModel
{
    public int HotelId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; }
    public int StarPoint { get; set; }
    public string ReviewMessage { get; set; }
    public List<string>? ReviewImageUrls { get; set; }
}
