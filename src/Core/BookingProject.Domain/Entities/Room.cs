using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class Room : BaseEntity, IBaseAuditable
{
    public DateTime CreatedDate { get;  }= DateTime.Now;

    public DateTime ModifiedDate { get ; set; }= DateTime.Now;
    public string RoomName { get; set; }
    public Hotel Hotel { get; set; }
    public int HotelId { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal Area {  get; set; }
    public bool IsCancellable { get; set; }
    public int? CancelAfterDay { get; set; }
    public bool IsReserved { get; set; }
}
