namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.PaymentMethod;

public class UpdatePaymentMethodViewModel
{
	public string PaymentMethodName { get; set; }
	public int? Id { get; set; }
	public bool IsDeactive { get; set; } = false;
}
