using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class Reservation : BaseEntity, IBaseAuditable
{
	public DateTime CreatedDate { get ; set; }=DateTime.Now;
	public DateTime ModifiedDate { get ; set; }=DateTime.Now;
    public int RoomId { get; set; }
    public string AppUserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsPaid { get; set; } = false;
    public bool IsCancelled { get; set; } = false;
    public Room Room { get; set; }
    public AppUser AppUser { get; set; }
    public decimal TotalPrice { get; set; }
    public int DiscountPercent { get; set; }
}
