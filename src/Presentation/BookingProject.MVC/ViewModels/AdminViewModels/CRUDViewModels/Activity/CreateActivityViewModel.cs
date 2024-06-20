using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Activity;

public class CreateActivityViewModel
{
	[MaxLength(50)]
	[Required]
	public string ActivityName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
