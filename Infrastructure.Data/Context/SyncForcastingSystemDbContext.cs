using ForecastingSystem.Domain.Models;
using ForecastingSystem.Domain.Models.Base;
using ForecastingSystem.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace ForecastingSystem.Infrastructure.Data.Context
{
    public class SyncForecastingSystemDbContext : DbContext
    {
        public SyncForecastingSystemDbContext()
        {
        }

        public SyncForecastingSystemDbContext(DbContextOptions<SyncForecastingSystemDbContext> options) : base(options)
        { }

        public virtual DbSet<DataSyncProcess> DataSyncProcesses { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectRate> ProjectRates { get; set; }
        public virtual DbSet<ProjectRateHistory> ProjectRateHistories { get; set; }
        public virtual DbSet<ProjectEmployeeManager> ProjectEmployeeManagers { get; set; }
        public virtual DbSet<Phase> Phases { get; set; }

        public override int SaveChanges()
        {
            // Get all the entities that inherit from AuditableEntity
            // and have a state of Added or Modified
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableBaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            // For each entity we will set the Audit properties
            foreach (var entityEntry in entries)
            {
                // If the entity state is Added let's set
                // the CreatedAt and CreatedBy properties
                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableBaseEntity)entityEntry.Entity).Created = DateTime.UtcNow;
                    ((AuditableBaseEntity)entityEntry.Entity).CreatedBy = "SyncForecastingSystem";
                }
                else
                {
                    // If the state is Modified then we don't want
                    // to modify the CreatedAt and CreatedBy properties
                    // so we set their state as IsModified to false
                    Entry((AuditableBaseEntity)entityEntry.Entity).Property(p => p.Created).IsModified = false;
                    Entry((AuditableBaseEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                // In any case we always want to set the properties
                // ModifiedAt and ModifiedBy
                ((AuditableBaseEntity)entityEntry.Entity).LastModified = DateTime.UtcNow;
                ((AuditableBaseEntity)entityEntry.Entity).LastModifiedBy = "SyncForecastingSystem";
            }
            // After we set all the needed properties
            // we call the base implementation of SaveChanges
            // to actually save our entities in the database
            return base.SaveChanges();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetAssembly(typeof(ClientConfiguration)));

            base.OnModelCreating(modelBuilder);
        }

    }
}
