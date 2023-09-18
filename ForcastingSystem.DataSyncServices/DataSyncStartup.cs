using ForecastingSystem.DataSyncServices.Interfaces;

namespace ForecastingSystem.DataSyncServices
{
    public class DataSyncStartup : IDataSyncStartup
    {
        IDataSync _dataSync;
        IDataSyncServiceLogger _dataSyncLogger;

        public DataSyncStartup(IDataSync dataSync, IDataSyncServiceLogger dataSyncServiceLogger)
        {
            _dataSyncLogger = dataSyncServiceLogger;
            _dataSync = dataSync;
        }

        public async Task SyncAllDataAsync(CancellationToken token)
        {
            _dataSyncLogger.LogInfo("DataSync Report");
            try
            {
                await _dataSync.Start(token);
            }
            catch (Exception ex)
            {
                _dataSyncLogger.LogError("Data Sync service has stop", ex);
            }

            _dataSyncLogger.LogInfo("DataSync Completed Succesfully");
        }

    }
}
