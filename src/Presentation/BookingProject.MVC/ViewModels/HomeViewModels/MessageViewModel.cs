using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.HomeViewModels;

public class MessageViewModel
{
	[MaxLength(50)]
	[Required]
	public string Name { get; set; }
	[MaxLength(100)]
	[Required]
	public string Email { get; set; }
	[MaxLength(1000)]
	[Required]
	public string MessageText { get; set; }
}
