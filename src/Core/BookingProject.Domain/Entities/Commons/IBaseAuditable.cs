namespace BookingProject.Domain.Entities.Commons;

public interface IBaseAuditable
{
    public DateTime CreatedDate { get; }
    public DateTime ModifiedDate { get; set; }
}
