using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ForecastingSystem.Domain.Models
{
    public class EmployeeTimesheetEntryConfiguration : IEntityTypeConfiguration<EmployeeTimesheetEntry>
    {
        public void Configure(EntityTypeBuilder<EmployeeTimesheetEntry> entity)
        {
            entity.ToTable("EmployeeTimesheetEntry");

            entity.Property(e => e.EmployeeTimesheetEntryId).UseIdentityColumn(); ;
            entity.Property(e => e.StartDate).HasColumnType("datetime2");
            entity.Property(e => e.EndDate).HasColumnType("datetime2");
            entity.Property(e => e.TimesheetUsername).HasMaxLength(255).IsRequired();
            entity.Property(e => e.ProjectName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.PhaseCode).HasMaxLength(255).IsRequired();
            entity.Property(e => e.RateName).HasMaxLength(255);
        }
    }
}
