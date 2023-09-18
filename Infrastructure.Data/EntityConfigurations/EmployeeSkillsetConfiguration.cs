using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class EmployeeSkillsetConfiguration : IEntityTypeConfiguration<EmployeeSkillset>
    {
        public void Configure(EntityTypeBuilder<EmployeeSkillset> entity)
        {
            entity.ToTable("EmployeeSkillset");

            entity.Property(e => e.EmployeeId).HasColumnType("int");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Note).IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeSkillsets)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeSkillset_Employee");

            entity.HasOne(d => d.Skillset).WithMany(p => p.EmployeeSkillsets)
                .HasForeignKey(d => d.SkillsetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeSkillset_Skillset");
        }
    }
}
