using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class EmployeeUtilisationNotesConfiguration : IEntityTypeConfiguration<EmployeeUtilisationNotes>
    {
        public void Configure(EntityTypeBuilder<EmployeeUtilisationNotes> entity)
        {
            entity.ToTable("EmployeeUtilisationNotes");
            entity.Property(e => e.EmployeeUtilisationNotesId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Employee)
                .WithOne(e => e.EmployeeUtilisationNotes)
                .HasForeignKey<EmployeeUtilisationNotes>(x => x.EmployeeId)
                .HasConstraintName("FK_EmployeeUtilisationNotes_Employee");

            entity.Property(e => e.ForecastWarning)
                .IsUnicode(false);

            entity.Property(e => e.InternalWorkNotes)
                .IsUnicode(false);

            entity.Property(e => e.OtherNotes)
                .IsUnicode(false);

            entity.HasKey(e => e.EmployeeUtilisationNotesId);
        }
    }
}
