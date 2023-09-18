using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class SkillsetCategoryConfiguration : IEntityTypeConfiguration<SkillsetCategory>
    {
        public void Configure(EntityTypeBuilder<SkillsetCategory> entity)
        {
            entity.ToTable("SkillsetCategory");

            entity.Property(e => e.CategoryName).IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
        }
    }
}
