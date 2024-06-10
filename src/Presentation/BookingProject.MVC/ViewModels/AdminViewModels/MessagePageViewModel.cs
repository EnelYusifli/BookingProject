using BookingProject.MVC.Models;
using BookingProject.MVC.ViewModels.AdminViewModels.CRUDViewModels.Message;

namespace BookingProject.MVC.ViewModels.AdminViewModels;

public class MessagePageViewModel
{
	public PaginatedList<MessageGetAllViewModel>? List {get;set;}
	public MessageReplyViewModel? Reply {get;set;}
}
