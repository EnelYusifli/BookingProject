using BookingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Persistence.Configurations;

public class StaffLanguageConfiguration : IEntityTypeConfiguration<StaffLanguage>
{
    public void Configure(EntityTypeBuilder<StaffLanguage> builder)
    {
        builder.Property(x => x.StaffLanguageName).IsRequired().HasMaxLength(50);
    }
}