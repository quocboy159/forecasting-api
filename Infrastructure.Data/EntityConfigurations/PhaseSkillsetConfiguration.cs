using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class PhaseSkillsetConfiguration : IEntityTypeConfiguration<PhaseSkillset>
    {
        public void Configure(EntityTypeBuilder<PhaseSkillset> entity)
        {
            entity.HasKey(e => e.PhaseSkillSetId).HasName("PK_ProjectSkillset");

            entity.ToTable("PhaseSkillset");

            entity.HasOne(d => d.Phase).WithMany(p => p.PhaseSkillsets)
                .HasForeignKey(d => d.PhaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhaseSkillset_Phase");

            entity.HasOne(d => d.Skillset).WithMany(p => p.PhaseSkillsets)
                .HasForeignKey(d => d.SkillsetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhaseSkillset_Skillset");
        }
    }
}
