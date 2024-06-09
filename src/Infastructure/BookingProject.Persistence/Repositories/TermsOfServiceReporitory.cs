using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class TermsOfServiceRepository : GenericRepository<TermsOfService>, ITermsOfServiceRepository
{
	public TermsOfServiceRepository(AppDbContext context) : base(context)
	{
	}
}
