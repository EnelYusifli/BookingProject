using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
	public ReservationRepository(AppDbContext context) : base(context)
	{
	}
}
