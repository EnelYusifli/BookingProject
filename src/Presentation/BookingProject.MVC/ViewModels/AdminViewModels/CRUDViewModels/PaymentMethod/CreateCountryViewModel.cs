using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.PaymentMethod;

public class CreatePaymentMethodViewModel
{
	[MaxLength(50)]
	[Required]
	public string PaymentMethodName { get; set; }
	public bool IsDeactive { get; set; }=false;
}
