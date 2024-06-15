using BookingProject.Domain.Entities.Commons;

namespace BookingProject.Domain.Entities;

public class Message : BaseEntity, IBaseAuditable
{
	public DateTime CreatedDate { get; set; }=DateTime.Now;
	public DateTime ModifiedDate { get; set; }=DateTime.Now;
	public string Name { get; set; }
	public string Email { get; set; }
	public bool IsReplied { get; set; } = false;
	public string MessageText { get; set; }
}
