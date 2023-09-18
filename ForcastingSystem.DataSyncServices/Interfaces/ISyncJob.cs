namespace ForecastingSystem.DataSyncServices.Interfaces
{
    public interface ISyncJob
    {
        string DataSyncType { get; }
        string Source { get; }
        string Target { get; }
        int SyncOrder { get; }
        Task SyncProcess(CancellationToken cancellationToken);
    }
}
