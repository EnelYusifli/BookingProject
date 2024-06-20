using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Activity;

public class UpdateActivityViewModel
{
	[MaxLength(50)]
	[Required]
	public string ActivityName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
