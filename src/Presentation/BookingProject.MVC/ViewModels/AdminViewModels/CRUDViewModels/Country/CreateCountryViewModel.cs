using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Country;

public class CreateCountryViewModel
{
	[MaxLength(50)]
	[Required]
	public string CountryName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
