using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class ResourceUtilisationNoteConfiguration : IEntityTypeConfiguration<ResourceUtilisationNote>
    {
        public void Configure(EntityTypeBuilder<ResourceUtilisationNote> entity)
        {
            entity.ToTable("ResourceUtilisationNote");
            entity.Property(e => e.ResourceUtilisationNoteId).ValueGeneratedOnAdd();
            entity.Property(e => e.Note).IsUnicode(true);
            entity.Property(e => e.StartWeek).HasColumnType("datetime2");
        }
    }
}
