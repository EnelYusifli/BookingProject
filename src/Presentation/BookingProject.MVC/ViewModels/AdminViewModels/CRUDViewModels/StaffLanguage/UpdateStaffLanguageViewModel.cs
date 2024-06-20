using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.StaffLanguage;

public class UpdateStaffLanguageViewModel
{
	[MaxLength(50)]
	[Required]
	public string StaffLanguageName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
