using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class ResourcePlaceHolderConfiguration : IEntityTypeConfiguration<ResourcePlaceHolder>
    {
        public void Configure(EntityTypeBuilder<ResourcePlaceHolder> entity)
        {
            entity.ToTable("ResourcePlaceHolder");
            entity.HasKey(e => e.ResourcePlaceHolderId).HasName("PK_ResourcePlaceHolderId");
            entity.Property(e => e.ResourcePlaceHolderName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
        }

    }
}
