namespace BookingProject.MVC.ViewModels.PropertyViewModels;

public class StaffLanguageGetViewModel
{
	public int Id { get; set; }
	public string staffLanguageName { get; set; }
	public bool IsDeactive { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime ModifiedDate { get; set; }
}
