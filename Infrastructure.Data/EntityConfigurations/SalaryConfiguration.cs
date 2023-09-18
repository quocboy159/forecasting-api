using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> entity)
        {
            entity.ToTable("Salary");

            entity.Property(e => e.SalaryId).ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeId).HasColumnType("int");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ExternalId).HasColumnName("ExternalID");
            entity.Property(e => e.LastSyncDate).HasColumnType("datetime2");
            entity.Property(e => e.Salary1).HasColumnName("Salary");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Employee).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salary_Employee");
        }
    }
}
