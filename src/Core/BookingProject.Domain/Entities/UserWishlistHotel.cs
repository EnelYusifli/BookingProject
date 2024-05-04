using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class UserWishlistHotel : BaseEntity, IBaseAuditable
{
    public DateTime CreatedDate { get; }= DateTime.Now;

    public DateTime ModifiedDate { get; set; }= DateTime.Now;
    public Hotel Hotel { get; set; }
    public AppUser User { get; set; }
    public int HotelId { get; set; }
    public string UserId { get; set; }
}
