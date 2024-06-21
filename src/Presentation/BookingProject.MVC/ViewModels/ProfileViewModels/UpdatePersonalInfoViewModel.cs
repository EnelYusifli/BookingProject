using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.ProfileViewModels;

public class UpdatePersonalInfoViewModel
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; }

    public string? Id { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters")]
    public string UserName { get; set; }

    [MaxLength(100, ErrorMessage = "Phone number cannot exceed 100 characters")]
    public string? PhoneNumber { get; set; }

    public DateOnly? Birthdate { get; set; }

    public IFormFile? ProfilePhoto { get; set; }
}
