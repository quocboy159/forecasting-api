namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface ITimesheetEntryService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.TimesheetEntry> timesheetEntries, CancellationToken cancellationToken);
        Task DeleteRangeAsync(IEnumerable<Outbound.TimesheetDeletedEntry> timesheetDeletedEntries, CancellationToken cancellationToken);
    }
}
