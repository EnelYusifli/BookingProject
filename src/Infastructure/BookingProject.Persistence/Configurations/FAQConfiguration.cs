using BookingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingProject.Persistence.Configurations;

public class FAQConfiguration: IEntityTypeConfiguration<FAQ>
{
	public void Configure(EntityTypeBuilder<FAQ> builder)
{
    builder.Property(x => x.Question).IsRequired().HasMaxLength(200);
    builder.Property(x => x.Answer).IsRequired().HasMaxLength(1000);
}
}
