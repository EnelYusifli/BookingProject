using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class FAQsRepository : GenericRepository<FAQ>, IFAQsRepository
{
    public FAQsRepository(AppDbContext context) : base(context)
    {
    }
}
