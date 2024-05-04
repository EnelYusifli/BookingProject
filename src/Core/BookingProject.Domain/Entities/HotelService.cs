using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class HotelService : BaseEntity, IBaseAuditable
{
    public int HotelId { get; set; }
    public int ServiceId { get; set; }
    public Hotel Hotel { get; set; }
    public Service Service { get; set; }

    public DateTime CreatedDate { get; } = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
}
