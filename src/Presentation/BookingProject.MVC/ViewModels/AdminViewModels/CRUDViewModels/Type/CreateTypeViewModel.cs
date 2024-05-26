namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Type;

public class CreateTypeViewModel
{
    public string TypeName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
