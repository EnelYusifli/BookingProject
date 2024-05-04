using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class HotelImage : BaseEntity, IBaseAuditable
{
    public DateTime CreatedDate { get; }=DateTime.Now;

    public DateTime ModifiedDate { get; set; }=DateTime.Now;
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public string Url { get; set; }
}
