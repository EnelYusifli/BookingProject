using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.StaffLanguage;

public class CreateStaffLanguageViewModel
{
	[MaxLength(50)]
	[Required]
	public string StaffLanguageName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
