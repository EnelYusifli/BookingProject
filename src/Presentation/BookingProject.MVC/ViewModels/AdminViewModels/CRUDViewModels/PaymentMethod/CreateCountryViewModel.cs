namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.PaymentMethod;

public class CreatePaymentMethodViewModel
{
    public string PaymentMethodName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
