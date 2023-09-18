using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IEmployeeTimesheetEntryRepository : IAsyncRepository<EmployeeTimesheetEntry>
    {
        Task<List<EmployeeTimesheetEntry>> GetEntriesToCalculateRevenue(List<int?> externalProjectIds, DateTime? startDateTime = null, DateTime? endDateTime = null);
        Task<List<EmployeeTimesheetEntry>> GetEntries(DateTime startDateTime, DateTime endDateTime);
    }
}
