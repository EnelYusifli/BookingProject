using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookingProject.Domain.Entities;

public class AppUser:IdentityUser,IUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? Birthdate { get; set; }
    public string? RecoveryEmail { get; set; }
    public string? PasswordResetToken { get; set; } = null;
    public DateTime? ResetTokenExpires { get; set; } = null;
    public string? ProfilePhotoUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
    public List<CustomerReview>? CustomerReviews { get; set; }
	public List<Reservation>? Reservation { get; set; }
	public List<UserWishlistHotel>? UserWishlistHotel { get; set; }
    public List<Hotel>? Hotels { get; set; }
    public List<UserCard>? Cards { get; set; }
}
