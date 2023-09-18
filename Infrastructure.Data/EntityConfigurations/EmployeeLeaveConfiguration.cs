using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class EmployeeLeaveConfiguration : IEntityTypeConfiguration<EmployeeLeave>
    {
        public void Configure(EntityTypeBuilder<EmployeeLeave> entity)
        {
            entity.ToTable("EmployeeLeave");
            entity.Property(e => e.EmployeeLeaveId)
                .UseIdentityColumn();
            entity.Property(e => e.TimesheetUsername)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(true);
            entity.Property(e => e.TotalDays).HasColumnType("float");
            entity.Property(e => e.StartDate).HasColumnType("datetime2");
            entity.Property(e => e.EndDate).HasColumnType("datetime2");
            entity.Property(e => e.DayType)
                .HasMaxLength(20)
                .IsUnicode(true);
            entity.Property(e => e.SubmissionDate).HasColumnType("datetime2");
            entity.Property(e => e.LeaveCode)
                .HasMaxLength(10)
                .IsUnicode(true);
            entity.Property(e => e.ExternalLeaveId);
        }
    }
}
