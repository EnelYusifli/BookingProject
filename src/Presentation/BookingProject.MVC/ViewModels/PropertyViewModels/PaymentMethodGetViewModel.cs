namespace BookingProject.MVC.ViewModels.PropertyViewModels;

public class PaymentMethodGetViewModel
{
	public int Id { get; set; }
	public string PaymentMethodName { get; set; }
	public bool IsDeactive { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime ModifiedDate { get; set; }
}
