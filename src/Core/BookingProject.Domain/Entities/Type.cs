using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class Type : BaseEntity, IBaseAuditable
{
       public DateTime CreatedDate { get; set;} = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public string TypeName { get; set; }
    public List<Hotel> Hotels { get; set; }
}

