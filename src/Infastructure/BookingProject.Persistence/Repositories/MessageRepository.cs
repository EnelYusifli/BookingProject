using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
	public MessageRepository(AppDbContext context) : base(context)
	{
	}
}
