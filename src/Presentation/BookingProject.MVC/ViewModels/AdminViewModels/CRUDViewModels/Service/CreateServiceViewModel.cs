namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Service;

public class CreateServiceViewModel
{
    public string ServiceName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
