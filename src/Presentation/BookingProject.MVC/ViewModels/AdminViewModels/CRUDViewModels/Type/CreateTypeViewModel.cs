using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Type;

public class CreateTypeViewModel
{
	[MaxLength(50)]
	[Required]
	public string TypeName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
