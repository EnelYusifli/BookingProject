namespace BookingProject.MVC.ViewModels.PropertyViewModels;

public class ServiceGetViewModel
{
	public int Id { get; set; }
	public string ServiceName { get; set; }
	public bool IsDeactive { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime ModifiedDate { get; set; }
}
