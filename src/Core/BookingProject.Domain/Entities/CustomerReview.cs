using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class CustomerReview : BaseEntity, IBaseAuditable
{
    public DateTime CreatedDate { get; set;} = DateTime.Now;
    public DateTime ModifiedDate { get ; set ; }=DateTime.Now;
    public AppUser User { get; set; }
    public Hotel Hotel { get; set; }
    public int HotelId { get; set; }
    public string UserId { get; set; }
    public int StarPoint {  get; set; }
    public string ReviewMessage { get; set; }
    public List<ReviewImage> ReviewImages { get; set; }
}
