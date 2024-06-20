using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.TermsOfService;

public class UpdateTermsOfServiceViewModel
{
	[MaxLength(200)]
	[Required]
	public string Title { get; set; }
	[MaxLength(5000)]
	[Required]
	public string Text { get; set; }
}
