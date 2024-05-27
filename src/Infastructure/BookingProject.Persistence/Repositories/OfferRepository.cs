using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class OfferRepository : GenericRepository<Offer>, IOfferRepository
{
	public OfferRepository(AppDbContext context) : base(context)
	{
	}
}
