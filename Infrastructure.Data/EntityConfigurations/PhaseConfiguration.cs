using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;
using System;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class PhaseConfiguration : IEntityTypeConfiguration<Phase>
    {
        public void Configure(EntityTypeBuilder<Phase> entity)
        {
            entity.ToTable("Phase");

            entity.Property(e => e.PhaseId).ValueGeneratedOnAdd();
            entity.Property(e => e.Budget).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime")
                .HasConversion(x => x, x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : null);
            entity.Property(e => e.PhaseName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhaseCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime")
                .HasConversion(x => x, x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : null); ;

            entity.HasOne(d => d.Project).WithMany(p => p.Phases)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phase_Project");

            entity.Property(x => x.Status)
                .HasMaxLength(50)
                .HasConversion(
                    x => x.ToString(),
                    x => (PhaseStatus)Enum.Parse(typeof(PhaseStatus), x)
                );
        }
    }
}
