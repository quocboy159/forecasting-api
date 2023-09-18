namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IProjectRateService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.ProjectRate> tsProjectRates, CancellationToken cancellationToken);
    }
}
