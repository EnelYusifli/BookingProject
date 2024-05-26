namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Activity;

public class CreateActivityViewModel
{
    public string ActivityName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
