namespace BookingProject.MVC.ViewModels.PropertyViewModels;

public class CountriesGetViewModel
{
	public int Id { get; set; }
	public string CountryName { get; set; }
	public bool IsDeactive { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime ModifiedDate { get; set; }
}
