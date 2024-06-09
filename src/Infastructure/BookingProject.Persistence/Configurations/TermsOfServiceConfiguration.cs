using BookingProject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingProject.Persistence.Configurations;

public class TermsOfServiceConfiguration : IEntityTypeConfiguration<TermsOfService>
{
	public void Configure(EntityTypeBuilder<TermsOfService> builder)
	{
		builder.Property(x => x.Text).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Title).IsRequired().HasMaxLength(5000);
	}
}
