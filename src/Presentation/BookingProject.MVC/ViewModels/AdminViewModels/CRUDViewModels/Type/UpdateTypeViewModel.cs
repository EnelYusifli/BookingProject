using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Type;

public class UpdateTypeViewModel
{
	[MaxLength(50)]
	[Required]
	public string TypeName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
