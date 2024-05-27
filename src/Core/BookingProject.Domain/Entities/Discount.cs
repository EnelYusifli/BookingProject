using BookingProject.Domain.Entities.Commons;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Domain.Entities;

public class Discount : BaseEntity, IBaseAuditable
{
	public DateTime CreatedDate { get; set; }=DateTime.Now;
	public DateTime ModifiedDate { get; set; }=DateTime.Now;
	[Required]
	public int RoomId { get; set; }
	public Room? Room { get; set; }
	[Range(0, 100, ErrorMessage = "Percent value must be between 0 and 100.")]
	[Required]
	public int Percent { get; set; }
	[Required]
	public DateTime StartTime { get; set; }
	[Required]
	public DateTime EndTime { get; set; }	
}
