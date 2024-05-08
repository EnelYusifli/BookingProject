using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class ReviewImage : BaseEntity, IBaseAuditable
{
    public DateTime CreatedDate { get; set;} = DateTime.Now;
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public int ReviewId { get; set; }
    public CustomerReview Review { get; set; }
    public string Url { get; set; }
}
