using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class ReviewRepository : GenericRepository<CustomerReview>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }
}
