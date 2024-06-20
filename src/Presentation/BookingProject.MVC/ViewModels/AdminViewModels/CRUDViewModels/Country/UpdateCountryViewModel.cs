using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Country;

public class UpdateCountryViewModel
{
	[MaxLength(50)]
	[Required]
	public string CountryName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
