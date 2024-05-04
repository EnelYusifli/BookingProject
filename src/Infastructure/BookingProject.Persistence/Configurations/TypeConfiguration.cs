using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookingProject.Domain.Entities;

namespace BookingProject.Persistence.Configurations;

public class TypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Type>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Type> builder)
    {
        builder.Property(x => x.TypeName).IsRequired().HasMaxLength(50);
    }
}