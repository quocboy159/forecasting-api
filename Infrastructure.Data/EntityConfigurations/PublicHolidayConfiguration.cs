using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class PublicHolidayConfiguration : IEntityTypeConfiguration<PublicHoliday>
    {
        public void Configure(EntityTypeBuilder<PublicHoliday> entity)
        {
            entity.HasKey(e => e.PublicHolidayId);

            entity.ToTable("PublicHoliday");

            entity.Property(e => e.PublicHolidayId).ValueGeneratedOnAdd();
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
        }
    }
}
