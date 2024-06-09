using BookingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingProject.Persistence.Configurations;

public class AboutConfiguration : IEntityTypeConfiguration<About>
{
	public void Configure(EntityTypeBuilder<About> builder)
	{
		builder.Property(x => x.Story).IsRequired().HasMaxLength(200);
		builder.Property(x => x.StoryTitle).IsRequired().HasMaxLength(5000);
	}
}
