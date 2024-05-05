using BookingProject.Application.Repositories;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class ActivityRepository : GenericRepository<Domain.Entities.Activity>, IActivityRepository
{
    public ActivityRepository(AppDbContext context) : base(context)
    {
    }
}
