using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class ReviewImageRepository : GenericRepository<ReviewImage>, IReviewImageRepository
{
	public ReviewImageRepository(AppDbContext context) : base(context)
	{
	}
}
