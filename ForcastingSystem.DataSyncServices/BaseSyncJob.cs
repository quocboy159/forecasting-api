using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices
{
    public abstract class BaseSyncJob: ISyncJob
    {
        protected IDataSyncProcessService _dataSyncProcessService;
        protected IDataSyncServiceLogger _dataSyncLogger;
        protected DatetimeHelper _dateTimeHelper;

        public BaseSyncJob(IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService)
        {
            _dataSyncLogger = dataSyncLogger;
            _dateTimeHelper = datetimeHelper;
            _dataSyncProcessService = dataSyncProcessService;
        }

        public virtual string DataSyncType => "SyncJob";

        public virtual string Source => "Source";

        public virtual string Target => "Target";

        public virtual int SyncOrder => 0;

        public async Task SyncProcess(CancellationToken cancellationToken)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncProcess started");

            var lastSyncInfo = await _dataSyncProcessService.GetByTypeAsync(DataSyncType, cancellationToken);
            lastSyncInfo ??= new DataSyncProcess
            {
                DataSyncType = DataSyncType,
                Source = Source,
                Target = Target,
                LastSyncDateTime = DateTime.MinValue
            };
            await _dataSyncProcessService.UpdateSyncProcessToInprogressAsync(lastSyncInfo, cancellationToken);
            var nextSyncTime = _dateTimeHelper.Now();

            try
            {
                await SyncData(cancellationToken, lastSyncInfo.LastSyncDateTime);
                await _dataSyncProcessService.UpdateSyncProcessToSuccessAsync(lastSyncInfo, nextSyncTime, cancellationToken);
                // Do we need this log?
                _dataSyncLogger.LogInfo($"{GetType().Name} - SyncProcess completed successfully");
            }
            catch (Exception ex)
            {
                _dataSyncLogger.LogError($"{GetType().Name} - SyncProcess failed", ex);
                _dataSyncProcessService.UpdateSyncProcessToFailedAsync(lastSyncInfo, ex.GetFinalMessage(), cancellationToken).Wait();
               
                //Continue other sync
                //throw;
            }
        }

        protected virtual Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            // Do we need this log?
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            return Task.CompletedTask;
        }
    }
}
