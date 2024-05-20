using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
	public CountryRepository(AppDbContext context) : base(context)
	{
	}
}
