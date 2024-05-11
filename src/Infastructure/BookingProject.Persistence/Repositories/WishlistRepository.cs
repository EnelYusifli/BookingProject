using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class WishlistRepository : GenericRepository<UserWishlistHotel>, IWishlistRepository
{
	public WishlistRepository(AppDbContext context) : base(context)
	{
	}
}
