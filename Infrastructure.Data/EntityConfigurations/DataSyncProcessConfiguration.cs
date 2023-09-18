using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class DataSyncProcessConfiguration : IEntityTypeConfiguration<DataSyncProcess>
    {
        public void Configure(EntityTypeBuilder<DataSyncProcess> builder)
        {
            builder.ToTable("DataSyncProcess");
            builder.Property(e => e.DataSyncProcessId).UseIdentityColumn();
            builder.Property(e => e.DataSyncType).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Source).HasMaxLength(100);
            builder.Property(e => e.Target).HasMaxLength(100);
            builder.Property(e => e.Status).HasMaxLength(100)
                .HasConversion(
                    x => x.ToString(),
                    x => (DataSyncProcessStatuses)Enum.Parse(typeof(DataSyncProcessStatuses), x)
                ); ;
            builder.Property(e => e.FinishDateTime).HasColumnType("datetime2"); ;
        }
    }
}
