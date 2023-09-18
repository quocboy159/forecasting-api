using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class PhaseResourceConfiguration : IEntityTypeConfiguration<PhaseResource>
    {
        public void Configure(EntityTypeBuilder<PhaseResource> entity)
        {
            entity.ToTable("PhaseResource");

            entity.Property(e => e.PhaseResourceId).ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeId).HasColumnType("int");
            entity.Property(e => e.HoursPerWeek).HasColumnType("float");
            entity.Property(e => e.FTE).HasColumnType("float");

            entity.HasOne(d => d.Employee).WithMany(p => p.PhaseResources)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhaseResource_Employee");

            entity.HasOne(d => d.Phase).WithMany(p => p.PhaseResources)
                .HasForeignKey(d => d.PhaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhaseResource_Phase");

            entity.HasOne(d => d.ProjectRate).WithMany(p => p.PhaseResources)
                .HasForeignKey(d => d.ProjectRateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhaseResource_ProjectRate");

            entity.HasOne(d => d.ResourcePlaceHolder).WithOne(p => p.PhaseResource)
                .HasForeignKey<ResourcePlaceHolder>(d => d.ResourcePlaceHolderId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
