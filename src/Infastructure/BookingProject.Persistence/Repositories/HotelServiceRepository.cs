using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class HotelServiceRepository : GenericRepository<HotelService>, IHotelServiceRepository
{
    public HotelServiceRepository(AppDbContext context) : base(context)
    {
    }
}
