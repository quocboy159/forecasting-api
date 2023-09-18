namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IProjectRateHistoryService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.ProjectRateHistory> tsProjectRateHistories, CancellationToken cancellationToken);
    }
}
