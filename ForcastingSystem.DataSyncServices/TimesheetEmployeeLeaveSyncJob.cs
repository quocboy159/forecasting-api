using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class TimesheetEmployeeLeaveSyncJob: BaseSyncJob
    {
        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly IEmployeeLeaveService _employeeLeaveService;
        public TimesheetEmployeeLeaveSyncJob(ITimesheetConnectionFactory timesheetConnectionFactory
            , IEmployeeLeaveService employeeLeaveService
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService)
            : base(dataSyncLogger, datetimeHelper, dataSyncProcessService)
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _employeeLeaveService = employeeLeaveService;
        }

        public override string DataSyncType => DataSyncTypes.TimesheetSyncEmployeeLeaveJob;

        public override string Source => DataSyncSources.TimesheetSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.TimesheetSyncEmployeeLeaveJobOrder;

        protected override async Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            var tsEmployeeLeaves = await timesheetApi.GetEmployeeLeaves(lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'"));
            await _employeeLeaveService.AddOrUpdateRangeAsync(tsEmployeeLeaves, cancellationToken);
        }
    }
}
