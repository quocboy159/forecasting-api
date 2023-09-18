using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class DefaultResourcePlaceHolderConfiguration : IEntityTypeConfiguration<DefaultResourcePlaceHolder>
    {
        public void Configure(EntityTypeBuilder<DefaultResourcePlaceHolder> entity)
        {
            entity.HasKey(e => e.DefaultResourcePlaceHolderId);

            entity.ToTable("DefaultResourcePlaceHolder");

            entity.Property(e => e.DefaultResourcePlaceHolderId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}
