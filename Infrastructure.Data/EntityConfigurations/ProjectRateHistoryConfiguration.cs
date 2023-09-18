using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;
using System;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{

    public class ProjectRateHistoryConfiguration : IEntityTypeConfiguration<ProjectRateHistory>
    {
        public void Configure(EntityTypeBuilder<ProjectRateHistory> entity)
        {
            entity.ToTable("ProjectRateHistory");
            entity.Property(e => e.ProjectRateHistoryId).ValueGeneratedOnAdd();
            entity.Property(e => e.StartDate).HasColumnType("datetime")
                .HasConversion(x => x, x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : null);

            entity.HasOne(d => d.RateNavigation).WithMany(p => p.ProjectRateHistories)
                .HasForeignKey(d => d.ProjectRateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectRateHistory_ProjectRate");
        }
    }
}


