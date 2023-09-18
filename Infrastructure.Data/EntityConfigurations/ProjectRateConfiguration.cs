using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;
using System;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class ProjectRateConfiguration : IEntityTypeConfiguration<ProjectRate>
    {
        public void Configure(EntityTypeBuilder<ProjectRate> entity)
        {
            entity.HasKey(e => e.ProjectRateId);

            entity.ToTable("ProjectRate");

            entity.Property(e => e.ProjectRateId).ValueGeneratedOnAdd();
            entity.Property(e => e.RateName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectRates)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectRate_Project");

            entity.Property(x => x.Status)
                .HasMaxLength(50)
                .HasConversion(
                    x => x.ToString(),
                    x => (ProjectRateStatus)Enum.Parse(typeof(ProjectRateStatus), x)
                )
                .HasDefaultValue(ProjectRateStatus.Active);
        }
    }
}
