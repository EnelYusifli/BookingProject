using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class Activity : BaseEntity, IBaseAuditable
{
       public DateTime CreatedDate { get; set;} = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public string ActivityName { get; set; }
    public List<HotelActivity> HotelActivities { get; set; }
}
