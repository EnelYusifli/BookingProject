using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities.Commons;
using BookingProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookingProject.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : BaseEntity, new()
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }
    public DbSet<T> Table => _context.Set<T>();

    public async Task CreateAsync(T entity)
     => await Table.AddAsync(entity);

    public void Delete(T entity)
    => Table.Remove(entity);
    public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, params string[]? includes)
    {
        var query = Table.AsQueryable();
        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return expression is not null ? await query.Where(expression).ToListAsync() : await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
   => await Table.FindAsync(id);
    public async Task CommitAsync() => await _context.SaveChangesAsync();
}
