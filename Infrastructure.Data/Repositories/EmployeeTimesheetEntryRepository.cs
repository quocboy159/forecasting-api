using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class EmployeeTimesheetEntryRepository : GenericAsyncRepository<EmployeeTimesheetEntry>, IEmployeeTimesheetEntryRepository
    { 
        public EmployeeTimesheetEntryRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<EmployeeTimesheetEntry>> GetEntriesToCalculateRevenue(List<int?> externalProjectIds, DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            var entryQuery = DbContext.Set<EmployeeTimesheetEntry>().AsNoTracking();
            if(externalProjectIds != null && externalProjectIds.Any())
            {
                entryQuery = entryQuery.Where(s => externalProjectIds.Contains(s.ExternalProjectId));
            }
            if (startDateTime.HasValue)
            {
                entryQuery = entryQuery.Where(s => s.StartDate >= startDateTime.Value);
            }
            if (endDateTime.HasValue)
            {
                entryQuery = entryQuery.Where(s => s.EndDate <= endDateTime.Value);
            }
            return entryQuery.ToListAsync();
        }
        public Task<List<EmployeeTimesheetEntry>> GetEntries(DateTime startDateTime, DateTime endDateTime)
        {
            var entryQuery = DbContext.Set<EmployeeTimesheetEntry>().AsNoTracking();
            entryQuery = entryQuery.Where(s => s.StartDate >= startDateTime && s.EndDate <= endDateTime);
            return entryQuery.ToListAsync();
        }
    }
}
