using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> entity)
        {
            entity.HasKey(e => e.RateId);

            entity.ToTable("Rate");

            entity.Property(e => e.RateId).ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedDateTime).HasColumnType("datetime2");
            entity.Property(e => e.RateName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18, 0)");
        }
    }
}
