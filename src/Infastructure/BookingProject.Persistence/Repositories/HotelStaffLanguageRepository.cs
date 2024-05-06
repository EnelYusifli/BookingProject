using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class HotelStaffLanguageRepository : GenericRepository<HotelStaffLanguage>, IHotelStaffLanguageRepository
{
    public HotelStaffLanguageRepository(AppDbContext context) : base(context)
    {
    }
}
