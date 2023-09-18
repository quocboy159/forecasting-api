using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class PhaseResourceUtilisationConfiguration : IEntityTypeConfiguration<PhaseResourceUtilisation>
    {
        public void Configure(EntityTypeBuilder<PhaseResourceUtilisation> entity)
        {
            entity.ToTable("PhaseResourceUtilisation");

            entity.Property(e => e.PhaseResourceUtilisationId).ValueGeneratedOnAdd();
            entity.Property(e => e.StartWeek).HasColumnType("datetime2");
            entity.Property(e => e.TotalHours).HasColumnType("float");
            entity.HasOne(d => d.PhaseResource).WithMany(p => p.PhaseResourceUtilisations)
                .HasForeignKey(d => d.PhaseResourceId);
            //entity.Property(e => e.Username).HasMaxLength(255).IsUnicode(false);
            //entity.Property(e => e.ResourcePlaceHolderName).HasMaxLength(100).IsUnicode(false);
            //entity.Property(e => e.Country).HasMaxLength(100).IsUnicode(false);
            //entity.Property(e => e.ProjectName).HasMaxLength(255).IsUnicode(false);
            //entity.Property(e => e.PhaseName).HasMaxLength(255).IsUnicode(false);
            //entity.Property(e => e.FTE).HasColumnType("float");
        }
    }
}
