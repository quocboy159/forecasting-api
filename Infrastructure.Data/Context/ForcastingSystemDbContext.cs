using ForecastingSystem.Domain.Models;
using ForecastingSystem.Domain.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ForecastingSystem.Infrastructure.Data.Context
{
    public class ForecastingSystemDbContext : DbContext
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly string _username;
        public ForecastingSystemDbContext()
        {
        }

        public ForecastingSystemDbContext(DbContextOptions<ForecastingSystemDbContext> options) : base(options)
        { }

        public ForecastingSystemDbContext(DbContextOptions<ForecastingSystemDbContext> options , IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;

            // Get the claims principal from the HttpContext
            var claimsPrincipal = httpContextAccessor.HttpContext?.User;
            // Get the username claim from the claims principal - if the user is not authenticated the claim will be null
            _username = claimsPrincipal?.Identities.FirstOrDefault()?.Name ?? "BackendSystem";
        }


        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectRate> ProjectRates { get; set; }
        public virtual DbSet<ProjectRateHistory> ProjectRateHistories { get; set; }
        public virtual DbSet<ProjectEmployeeManager> ProjectEmployeeManagers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<PhaseResource> PhaseResources { get; set; }
        public virtual DbSet<Phase> Phases { get; set; }
        public virtual DbSet<PhaseResourceException> PhaseResourceExceptions { get; set; }
        public virtual DbSet<ResourcePlaceHolder> ResourcePlaceHolders { get; set; }
        public virtual DbSet<PublicHoliday> PublicHolidays { get; set; }
        public virtual DbSet<DataSyncProcess> DataSyncProcesses { get; set; }
        public virtual DbSet<DefaultResourcePlaceHolder> DefaultResourcePlaceHolders { get; set; }
        public virtual DbSet<UserIdLookup> UserIdLookups { get; set; }
        public virtual DbSet<EmployeeUtilisationNotes> EmployeeUtilisationNotes { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }

        public override int SaveChanges()
        {
            // Get audit entries
            var auditEntries = OnBeforeSaveChanges();
            // Save current entity
            var result = base.SaveChanges();
            // Save audit entries
            OnAfterSaveChangesAsync(auditEntries).Wait();
            return result;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess , CancellationToken cancellationToken = default)
        {
            // Get audit entries
            var auditEntries = OnBeforeSaveChanges();

            // Save current entity
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess , cancellationToken);

            // Save audit entries
            await OnAfterSaveChangesAsync(auditEntries);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var entries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                // Dot not audit entities that are not tracked, not changed, or not of type IAuditable
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || !(entry.Entity is IAuditable))
                    continue;

                var auditEntry = new AuditEntry
                {
                    ActionType = entry.State == EntityState.Added ? "INSERT" : entry.State == EntityState.Deleted ? "DELETE" : "UPDATE" ,
                    EntityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue.ToString() ,
                    EntityName = entry.Metadata.ClrType.Name ,
                    Username = _username ,
                    TimeStamp = DateTime.UtcNow ,
                    Changes = entry.Properties.Select(p => new { p.Metadata.Name , p.CurrentValue }).ToDictionary(i => i.Name , i => i.CurrentValue) ,

                    // TempProperties are properties that are only generated on save, e.g. ID's
                    // These properties will be set correctly after the audited entity has been saved
                    TempProperties = entry.Properties.Where(p => p.IsTemporary).ToList() ,
                };

                entries.Add(auditEntry);
            }

            return entries;
        }

        private Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            // For each temporary property in each audit entry - update the value in the audit entry to the actual (generated) value
            foreach (var entry in auditEntries)
            {
                foreach (var prop in entry.TempProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        entry.EntityId = prop.CurrentValue.ToString();
                        entry.Changes[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        entry.Changes[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
            }

            AuditEntries.AddRange(auditEntries);
            return SaveChangesAsync();
        }

    }
}
