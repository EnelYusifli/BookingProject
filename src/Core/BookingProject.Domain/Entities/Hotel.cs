using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class Hotel : BaseEntity, IBaseAuditable
{
    public int TypeId { get; set; }
    public int CountryId { get; set; }
    public DateTime CreatedDate { get; set;} = DateTime.Now;
    public DateTime ModifiedDate { get; set; }=DateTime.Now;
    public string AppUserId { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public bool IsApproved { get; set; } = false;
    public bool IsRefused { get; set; } = false;
    public int? ViewerCount { get; set; } = 0;
    public decimal? StarPoint { get; set; } = 0;
    public AppUser AppUser { get; set; }
    public List<HotelAdvantage> HotelAdvantages { get; set; }
    public List<HotelImage> HotelImages { get; set; }
    public List<HotelStaffLanguage> HotelStaffLanguages { get; set; }
    public List<HotelService> HotelServices { get; set; } 
    public List<HotelPaymentMethod> HotelPaymentMethods { get; set; } 
    public List<HotelActivity > HotelActivities { get; set; } 
    public List<CustomerReview > CustomerReviews { get; set; }
    public List<UserWishlistHotel> UserWishlistHotel { get; set; }
    public List<Room> Rooms { get; set; }
    public Type Type { get; set; }
    public Country Country { get; set; }
}
