using BookingProject.Domain.Entities;

namespace BookingProject.Application.Repositories;

public interface IUserRepository
{
	Task<AppUser> GetByToken(string token);
	Task CommitAsync();
}
