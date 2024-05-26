namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Type;

public class UpdateTypeViewModel
{
	public string TypeName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
