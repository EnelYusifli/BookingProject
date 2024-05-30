using System.Text;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class ReviewGetViewModel
{
    public int HotelId { get; set; }
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; }
    public int StarPoint { get; set; }
    public bool IsReported { get; set; }
    public string UserName { get; set; }
    public string UserPpUrl { get; set; }
    public bool IsDeactive { get; set; }
    public string ReviewMessage { get; set; }
    public List<string>? ReviewImageUrls { get; set; }
	public string HotelName { get; set; }

    public string GetStarRatingHtml()
    {
        int fullStars = (int)StarPoint;
        int halfStars = (StarPoint - fullStars) >= 0.5 ? 1 : 0;
        int emptyStars = 5 - fullStars - halfStars;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < fullStars; i++)
        {
            sb.Append("<i class='fas fa-star text-warning'></i>");
        }
        for (int i = 0; i < halfStars; i++)
        {
            sb.Append("<i class='fas fa-star-half-alt text-warning'></i>");
        }
        for (int i = 0; i < emptyStars; i++)
        {
            sb.Append("<i class='far fa-star text-warning'></i>");
        }

        return sb.ToString();
    }
}
