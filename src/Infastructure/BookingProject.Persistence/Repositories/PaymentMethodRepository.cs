using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class PaymentMethodRepository : GenericRepository<PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(AppDbContext context) : base(context)
    {
    }
}
