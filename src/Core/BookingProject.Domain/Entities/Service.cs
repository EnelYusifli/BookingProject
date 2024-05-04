using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class Service : BaseEntity, IBaseAuditable
{
    public DateTime CreatedDate { get; } = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public string ServiceName { get; set; }
    public List<HotelService> HotelServices { get; set; }
}
