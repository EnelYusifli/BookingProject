using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.FAQ;

public class CreateFAQViewModel
{
    [MaxLength(200)]
    [Required]
    public string Question { get; set; }
    [MaxLength(1000)]
    [Required]
    public string Answer { get; set; }
}
