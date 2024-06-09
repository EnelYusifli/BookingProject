using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class TermsOfService:BaseEntity
{
   public string Title { get; set; }
   public string Text { get; set; }
}
