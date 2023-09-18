using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class UserIdLookupConfiguration : IEntityTypeConfiguration<UserIdLookup>
    {
        public void Configure(EntityTypeBuilder<UserIdLookup> entity)
        {
            entity.ToTable("UserIdLookup");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.BambooHRFirstName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.BambooHRLastName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.BambooHREmail)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.TimesheetUserName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.TimesheetEmail)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.LastUpdatedDateTime).HasColumnType("datetime2");

            entity.HasKey(e => e.Id);
        }
    }
}
