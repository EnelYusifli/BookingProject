using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.About;

public class UpdateAboutViewModel
{
	[MaxLength(200)]
	[Required]
	public string StoryTitle { get; set; }
	[MaxLength(5000)]
	[Required]
	public string Story { get; set; }
}
