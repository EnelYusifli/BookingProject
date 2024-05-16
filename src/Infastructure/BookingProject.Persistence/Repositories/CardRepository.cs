using BookingProject.Application.Repositories;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class CardRepository : GenericRepository<UserCard>, ICardRepository
{
	public CardRepository(AppDbContext context) : base(context)
	{
	}
}
