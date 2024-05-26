namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.StaffLanguage;

public class UpdateStaffLanguageViewModel
{
	public string StaffLanguageName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
