using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class HotelImageRepository : GenericRepository<HotelImage>, IHotelImageRepository
{
    public HotelImageRepository(AppDbContext context) : base(context)
    {
    }
}
