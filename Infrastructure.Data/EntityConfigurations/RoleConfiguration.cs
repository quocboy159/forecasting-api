using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.RoleName)
                        .HasMaxLength(50)
                        .IsUnicode(false);
         
            entity.HasKey(e => e.RoleId);
        }
    }
}
