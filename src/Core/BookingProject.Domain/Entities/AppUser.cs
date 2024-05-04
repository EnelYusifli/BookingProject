using Microsoft.AspNetCore.Identity;

namespace BookingProject.Domain.Entities;

public class AppUser:IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? Birthdate { get; set; }
    public string? RecoveryEmail { get; set; }
    public List<CustomerReview> CustomerReviews { get; set; }
    public List<UserWishlistHotel> UserWishlistHotel { get; set; }
}
