using BookingProject.MVC.ViewModels.AccountViewModels;

namespace BookingProject.MVC.Services;

public interface ILoginService
{
    Task LoginUser(LoginViewModel request);
    Task LoginWithGoogle();
    Task RegisterWithGoogle();
}
