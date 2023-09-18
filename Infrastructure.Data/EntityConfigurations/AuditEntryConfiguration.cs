using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class AuditEntryConfiguration : IEntityTypeConfiguration<AuditEntry>
    {
        public void Configure(EntityTypeBuilder<AuditEntry> entity)
        {
            entity.ToTable("AuditEntry");
            entity.HasKey(e => e.AuditEntryId);
            entity.Property(e => e.AuditEntryId).ValueGeneratedOnAdd();
            entity.Ignore(e => e.TempProperties);

            entity.Property(ae => ae.Changes).HasConversion(
                value => JsonConvert.SerializeObject(value) ,
                serializedValue => JsonConvert.DeserializeObject<Dictionary<string , object>>(serializedValue));
        }
    }
}
