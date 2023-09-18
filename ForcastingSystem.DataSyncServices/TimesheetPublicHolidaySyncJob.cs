using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class TimesheetPublicHolidaySyncJob : BaseSyncJob
    {
        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly IPublicHolidayService _publicHolidayService;
        public TimesheetPublicHolidaySyncJob(ITimesheetConnectionFactory timesheetConnectionFactory 
            , IPublicHolidayService publicHolidayService
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService)
            : base(dataSyncLogger, datetimeHelper, dataSyncProcessService)
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _publicHolidayService = publicHolidayService;
        }

        public override string DataSyncType => DataSyncTypes.TimesheetSyncPublicHolidayJob;

        public override string Source => DataSyncSources.TimesheetSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.TimesheetSyncPublicHolidayJobOrder;

        protected override async Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            var timesheetPublicHolidays = await timesheetApi.GetPublicHolidays(lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'"));
            await _publicHolidayService.AddOrUpdateRangeAsync(timesheetPublicHolidays, cancellationToken);
        }
    }
}
