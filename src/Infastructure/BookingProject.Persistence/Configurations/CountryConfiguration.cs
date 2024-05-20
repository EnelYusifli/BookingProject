using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookingProject.Domain.Entities;

namespace BookingProject.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
	public void Configure(EntityTypeBuilder<Country> builder)
	{
		builder.Property(x => x.CountryName).IsRequired().HasMaxLength(50);
	}
}