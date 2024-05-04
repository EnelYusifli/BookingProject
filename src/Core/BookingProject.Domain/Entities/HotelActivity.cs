using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class HotelActivity:BaseEntity,IBaseAuditable
{
    public int HotelId { get; set; }
    public int ActivityId { get; set; }
    public Hotel Hotel { get; set; }
    public Activity Activity { get; set; }

    public DateTime CreatedDate { get; }=DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
}
