using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            entity.ToTable("Client");
            entity.HasIndex(e => e.ClientName).IsUnique();
            entity.Property(e => e.ClientId).ValueGeneratedOnAdd();
            entity.Property(e => e.ClientName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ClientType)
                        .HasMaxLength(50)
                        .IsUnicode(false);
            entity.Property(e => e.ClientCode)
                       .HasMaxLength(10)
                       .IsUnicode(false);

            entity.HasKey(e => e.ClientId);
        }
    }
}
