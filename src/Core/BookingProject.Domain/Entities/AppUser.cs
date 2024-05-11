using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookingProject.Domain.Entities;

public class AppUser:IdentityUser,IUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? Birthdate { get; set; }
    public string? RecoveryEmail { get; set; }
    public string ProfilePhotoUrl { get; set; } = "https://www.shutterstock.com/image-vector/vector-flat-illustration-grayscale-avatar-600nw-2281862025.jpg";
    public List<CustomerReview>? CustomerReviews { get; set; }
    public List<UserWishlistHotel>? UserWishlistHotel { get; set; }
    public List<Hotel>? Hotels { get; set; }
}
