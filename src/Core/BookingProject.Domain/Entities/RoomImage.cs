using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class RoomImage : BaseEntity, IBaseAuditable
{
       public DateTime CreatedDate { get; set;} = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public string Url { get; set; }
}
