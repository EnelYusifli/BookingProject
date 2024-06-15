namespace BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Message;

public class MessageGetAllViewModel
{
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public string Name { get; set; }
    public int Id { get; set; }
	public bool IsReplied { get; set; }
	public string Email { get; set; }
    public string MessageText { get; set; }
}
