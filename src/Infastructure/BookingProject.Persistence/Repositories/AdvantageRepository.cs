using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class AdvantageRepository : GenericRepository<HotelAdvantage>, IAdvantageRepository
{
    public AdvantageRepository(AppDbContext context) : base(context)
    {
    }
}
