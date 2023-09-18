using ForecastingSystem.Domain.Interfaces.SyncInterfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.Data.SyncRepositories
{
    public class SyncEmployeeTimesheetEntryRepository : SyncGenericAsyncRepository<EmployeeTimesheetEntry>, ISyncEmployeeTimesheetEntryRepository
    {
        public SyncEmployeeTimesheetEntryRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
