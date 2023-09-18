using ForecastingSystem.DataSyncServices.Interfaces;

namespace ForecastingSystem.DataSyncServices
{
    public class DataSyncService : IDataSync
    {
        private IDataSyncServiceLogger _dataSyncLogger;
        private IEnumerable<ISyncJob> _syncJobs = new List<ISyncJob>();

        public DataSyncService(IDataSyncServiceLogger dataSyncLogger
            , IEnumerable<ISyncJob> syncJobs)
        {
            _dataSyncLogger = dataSyncLogger;
            _syncJobs = syncJobs;
        }

        public async Task Start(CancellationToken token)
        {
            _dataSyncLogger.StartNewReport();
            //_dataSyncLogger.LogInfo($"{GetType().Name} begin");
            bool hasSomeException = false;
            foreach (var job in _syncJobs.OrderBy(x => x.SyncOrder))
            {
                try
                {
                    await job.SyncProcess(token);
                }
                catch (Exception ex)
                {
                    hasSomeException = true;
                    _dataSyncLogger.LogError($"{GetType().Name} has failed at {job.GetType().Name}" , ex);

                    ////TODO: dangle - continue or not!                    
                    //break;
                }
            }

            if (hasSomeException)
            {
                _dataSyncLogger.LogWarning($"{GetType().Name} has finished with some Exception");
            }
            //else
            //{
            //    _dataSyncLogger.LogInfo($"{GetType().Name} has finished Succesfully");
            //}
            _dataSyncLogger.EndReport();
        }
    }
}
