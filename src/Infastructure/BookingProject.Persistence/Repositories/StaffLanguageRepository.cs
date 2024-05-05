using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class StaffLanguageRepository : GenericRepository<StaffLanguage>, IStaffLanguageRepository
{
    public StaffLanguageRepository(AppDbContext context) : base(context)
    {
    }
}
