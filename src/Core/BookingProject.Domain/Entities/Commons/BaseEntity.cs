namespace BookingProject.Domain.Entities.Commons;

public class BaseEntity
{
    public int Id { get; set; }
    public bool IsDeactive { get; set; } = false;
}
