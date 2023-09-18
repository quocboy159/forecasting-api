using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class TimesheetDeletedEntrySyncJob : BaseSyncJob
    {
        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly ITimesheetEntryService _timesheetEntryService;
        public TimesheetDeletedEntrySyncJob(ITimesheetConnectionFactory timesheetConnectionFactory
            , ITimesheetEntryService timesheetEntryService
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService)
            : base(dataSyncLogger, datetimeHelper, dataSyncProcessService)
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _timesheetEntryService = timesheetEntryService;
        }

        public override string DataSyncType => DataSyncTypes.TimesheetSyncDeletedEntryJob;

        public override string Source => DataSyncSources.TimesheetSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.TimesheetSyncDeletedEntryJobOrder;

        protected override async Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            var tsEntries = await timesheetApi.GetTimesheetDeletedEntries(lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'"));
            if (tsEntries.Length > 0)
            {
                await _timesheetEntryService.DeleteRangeAsync(tsEntries, cancellationToken);
            }
        }
    }
}

