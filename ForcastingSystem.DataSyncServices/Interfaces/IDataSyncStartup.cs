namespace ForecastingSystem.DataSyncServices.Interfaces
{
    public interface IDataSyncStartup
    {
        Task SyncAllDataAsync(CancellationToken token);
    }
}
