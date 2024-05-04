using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class HotelAdvantage:BaseEntity,IBaseAuditable
{
    public Hotel Hotel { get; set; }
    public int HotelId { get; set;}
    public string AdvantageName { get; set; }
    public DateTime CreatedDate { get;}=DateTime.Now;
    public DateTime ModifiedDate { get ; set; }=DateTime.Now;
}
