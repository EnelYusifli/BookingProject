using BookingProject.Domain.Entities.Commons;
using BookingProject.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class UserCard : BaseEntity, IBaseAuditable
{
	[Required(ErrorMessage = "Created date is required.")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	[Required(ErrorMessage = "Modified date is required.")]
	public DateTime ModifiedDate { get; set; } = DateTime.Now;

	[Required(ErrorMessage = "Card number is required.")]
	[RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid card number format.")]
	public string CardNumber { get; set; }

	[Required(ErrorMessage = "CVC is required.")]
	[RegularExpression(@"^\d{3,4}$", ErrorMessage = "Invalid CVC format.")]
	public string CVC { get; set; }


	[Required(ErrorMessage = "Expiration month is required.")]
	[Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
	public int ExpireMonth { get; set; }

	[Required(ErrorMessage = "Expiration year is required.")]
	[Range(2024, 2100, ErrorMessage = "Year must be between 2022 and 2100.")]
	public int ExpireYear { get; set; }

	[Required(ErrorMessage = "User ID is required.")]
	public string AppUserId { get; set; }
	public AppUser AppUser { get; set; }
}
