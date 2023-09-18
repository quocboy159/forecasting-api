namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IPhaseService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.Phase> tsPhases, CancellationToken cancellationToken);
    }
}
