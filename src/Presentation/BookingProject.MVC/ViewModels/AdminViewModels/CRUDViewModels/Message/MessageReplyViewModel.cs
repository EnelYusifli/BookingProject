using System.ComponentModel.DataAnnotations;

namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Message;

public class MessageReplyViewModel
{
    public int Id { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Reply { get; set; }
}
