namespace ForecastingSystem.DataSyncServices.Interfaces
{
    public interface IDataSync
    {
        Task Start(CancellationToken token);
    }
}
