namespace BookingProject.MVC.ViewModels.PropertyViewModels;

public class ActivityGetViewModel
{
	public int Id { get; set; }
	public string ActivityName { get; set; }
	public bool IsDeactive { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime ModifiedDate { get; set; }
}
