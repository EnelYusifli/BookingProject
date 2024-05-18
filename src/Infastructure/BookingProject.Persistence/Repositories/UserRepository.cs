using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using BookingProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Persistence.Repositories;

public class UserRepository : IUserRepository
{
	private readonly AppDbContext _context;

	public UserRepository(AppDbContext context)
    {
		_context = context;
	}

	public async Task CommitAsync()
	{
		await _context.SaveChangesAsync();
	}

	public async Task<AppUser> GetByToken(string token)
	{
		AppUser user=await _context.Users.FirstOrDefaultAsync(x=>x.PasswordResetToken==token);
		return user;
	}
}
