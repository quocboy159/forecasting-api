using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class SkillsetConfiguration : IEntityTypeConfiguration<Skillset>
    {
        public void Configure(EntityTypeBuilder<Skillset> entity)
        {
            entity.ToTable("Skillset");

            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.SkillsetName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SkillsetTypeId)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.SkillsetCategory).WithMany(p => p.Skillsets)
                .HasForeignKey(d => d.SkillsetCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Skillset_SkillsetCategory");
        }
    }
}
