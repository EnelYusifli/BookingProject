using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class HotelPaymentMethodRepository : GenericRepository<HotelPaymentMethod>, IHotelPaymentMethodRepository
{
    public HotelPaymentMethodRepository(AppDbContext context) : base(context)
    {
    }
}
