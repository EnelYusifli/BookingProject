using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Service;

public class CreateServiceViewModel
{
	[MaxLength(50)]
	[Required]
	public string ServiceName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
