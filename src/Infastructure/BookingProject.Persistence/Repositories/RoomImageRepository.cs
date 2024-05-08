using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class RoomImageRepository : GenericRepository<RoomImage>, IRoomImageRepository
{
    public RoomImageRepository(AppDbContext context) : base(context)
    {
    }
}
