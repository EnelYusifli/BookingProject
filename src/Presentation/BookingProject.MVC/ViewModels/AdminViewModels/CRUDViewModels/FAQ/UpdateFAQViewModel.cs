using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.FAQ;

public class UpdateFAQViewModel
{
    [Required]
    [MaxLength(200)]
    public string Question { get; set; }
    public int Id { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Answer { get; set; }
}
