namespace BookingProject.MVC.Services;

public interface IPayPalService
{
	public Task<string> CreateOrderAsync(decimal amount, string currency);
}
