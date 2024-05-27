using BookingProject.Domain.Entities.Commons;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Domain.Entities;

public class Offer : BaseEntity, IBaseAuditable
{
	public DateTime CreatedDate { get; set; }= DateTime.Now;
	public DateTime ModifiedDate { get ; set; }=DateTime.Now;
	[Range(0, 100, ErrorMessage = "Percent value must be between 0 and 100.")]
	[Required]
	public int Percent { get; set; }
	[MaxLength(20)]
	[Required]
	public string Code { get; set; }
	[Required]
	public  DateTime StartTime { get; set; }
	[Required]
	public  DateTime EndTime { get; set; }
	public List<AppUser>? AppUsers { get; set; }
}
