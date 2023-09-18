using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Domain.Interfaces.SyncInterfaces
{
    public interface ISyncEmployeeTimesheetEntryRepository : IAsyncRepository<EmployeeTimesheetEntry>
    {
    }
}
