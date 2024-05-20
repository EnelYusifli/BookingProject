namespace BookingProject.MVC.ViewModels.PropertyViewModels;

public class PropertyViewModel
{
    public List<TypeGetViewModel> Types { get; set; }
    public List<CountriesGetViewModel> Countries { get; set; }
    public List<ActivityGetViewModel> Activities { get; set; }
    public List<PaymentMethodGetViewModel> PaymentMethods { get; set; }
    public List<StaffLanguageGetViewModel> StaffLanguages { get; set; }
    public List<ServiceGetViewModel> Services { get; set; }
}
