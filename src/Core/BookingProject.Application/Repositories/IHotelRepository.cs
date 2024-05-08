using BookingProject.Domain.Entities;

namespace BookingProject.Application.Repositories;

public interface IHotelRepository:IGenericRepository<Hotel>
{
	Task<decimal> GetTotalStarPointsAsync(int hotelId);
	Task<int> GetNumberOfReviewsAsync(int hotelId);
}
