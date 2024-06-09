using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class About:BaseEntity
{
    public string StoryTitle { get; set; }
    public string Story { get; set; }
}
