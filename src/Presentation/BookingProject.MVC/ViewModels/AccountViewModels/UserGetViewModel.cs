using BookingProject.Domain.Entities;
using System.Security.Claims;

namespace BookingProject.MVC.ViewModels.AccountViewModels;

public class UserGetViewModel
{
	public ClaimsPrincipal User { get; set; }
}
