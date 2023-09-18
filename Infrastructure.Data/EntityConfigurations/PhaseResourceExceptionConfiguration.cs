using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class PhaseResourceExceptionConfiguration : IEntityTypeConfiguration<PhaseResourceException>
    {
        public void Configure(EntityTypeBuilder<PhaseResourceException> entity)
        {
            entity.HasKey(e => e.PhaseResourceExceptionId).HasName("PK_PhaseResourceExceptionId");

            entity.ToTable("PhaseResourceException");

            entity.Property(e => e.PhaseResourceExceptionId).ValueGeneratedOnAdd();
           
            entity.Property(e => e.StartWeek).HasColumnType("date");        
        }
    }
}
