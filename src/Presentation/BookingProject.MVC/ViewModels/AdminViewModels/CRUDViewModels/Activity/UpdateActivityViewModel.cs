namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Activity;

public class UpdateActivityViewModel
{
	public string ActivityName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
