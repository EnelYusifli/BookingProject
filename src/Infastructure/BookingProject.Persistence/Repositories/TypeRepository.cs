using BookingProject.Application.Repositories;
using BookingProject.Persistence.Contexts;

namespace BookingProject.Persistence.Repositories;

public class TypeRepository : GenericRepository<Domain.Entities.Type>, ITypeRepository
{
    public TypeRepository(AppDbContext context) : base(context)
    {
    }
}
