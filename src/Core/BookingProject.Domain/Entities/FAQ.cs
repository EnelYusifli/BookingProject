using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class FAQ:BaseEntity
{
    public string Question { get; set; }
    public string Answer { get; set; }
}
