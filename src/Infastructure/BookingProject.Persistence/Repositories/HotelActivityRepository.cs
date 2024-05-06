using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class HotelActivityRepository : GenericRepository<HotelActivity>, IHotelActivityRepository
{
    public HotelActivityRepository(AppDbContext context) : base(context)
    {
    }
}
