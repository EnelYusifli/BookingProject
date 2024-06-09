using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class AboutRepository : GenericRepository<About>, IAboutRepository
{
	public AboutRepository(AppDbContext context) : base(context)
	{
	}
}
