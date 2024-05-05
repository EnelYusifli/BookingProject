using BookingProject.Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookingProject.Application.Repositories;

public interface IGenericRepository<T> where T : BaseEntity, new()
{
    DbSet<T> Table { get; }
    Task CreateAsync(T entity);
    void Delete(T entity);
    Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, params string[]? includes);
    Task<T> GetByIdAsync(int id);
    Task CommitAsync();
}
