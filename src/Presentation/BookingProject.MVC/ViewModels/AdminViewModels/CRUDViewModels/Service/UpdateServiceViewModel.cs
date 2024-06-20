using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Service;

public class UpdateServiceViewModel
{
	[MaxLength(50)]
	[Required]
	public string ServiceName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
