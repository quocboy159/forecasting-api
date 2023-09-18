using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForecastingSystem.Infrastructure.Data.EntityConfigurations
{
    public class ProjectEmployeeManagerConfiguration : IEntityTypeConfiguration<ProjectEmployeeManager>
    {
        public void Configure(EntityTypeBuilder<ProjectEmployeeManager> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable(nameof(ProjectEmployeeManager));
         
            entity.HasOne(d => d.Project).WithMany(p => p.ProjectEmployeeManagers)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_ProjectEmployeeManager_Project");

            entity.HasOne(d => d.Employee).WithMany(p => p.ProjectEmployeeManagers)
               .HasForeignKey(d => d.EmployeeId)
               .OnDelete(DeleteBehavior.ClientCascade)
               .HasConstraintName("FK_ProjectEmployeeManager_Employee");

            entity.HasIndex(d => new { d.ProjectId, d.EmployeeId }).IsUnique();
        }
    }
}
