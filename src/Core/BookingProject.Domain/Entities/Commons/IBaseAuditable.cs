namespace BookingProject.Domain.Entities.Commons;

public interface IBaseAuditable
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
