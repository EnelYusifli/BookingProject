using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Persistence.Repositories;

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
	private readonly AppDbContext _context;

	public HotelRepository(AppDbContext context) : base(context)
    {
		_context = context;
	}
	public async Task<decimal> GetTotalStarPointsAsync(int hotelId)
	{
		return await _context.CustomerReviews
			.Where(review => review.HotelId == hotelId)
			.SumAsync(review => review.StarPoint);
	}

	public async Task<int> GetNumberOfReviewsAsync(int hotelId)
	{
		return await _context.CustomerReviews
			.CountAsync(review => review.HotelId == hotelId);
	}
}
