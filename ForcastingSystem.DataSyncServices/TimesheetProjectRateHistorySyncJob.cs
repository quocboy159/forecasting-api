using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class TimesheetProjectRateHistorySyncJob : BaseSyncJob
    {
        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly IProjectRateHistoryService _projectRateHistoryService;
        private readonly IProjectService _projectService;
        public TimesheetProjectRateHistorySyncJob(
            ITimesheetConnectionFactory timesheetConnectionFactory
            , IProjectRateHistoryService projectRateHistoryService
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService
            , IProjectService projectService)
            : base(dataSyncLogger , datetimeHelper , dataSyncProcessService)
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _projectRateHistoryService = projectRateHistoryService;
            _projectService = projectService;
        }

        public override string DataSyncType => DataSyncTypes.TimesheetSyncProjectRateHistoryJob;

        public override string Source => DataSyncSources.TimesheetSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.TimesheetSyncProjectRateHistoryJobOrder;

        protected override async Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            List<int> externalProjectIds = await _projectService.GetSyncedProjectExternalIds();
            var projectIds = string.Join<int>("," , externalProjectIds);

            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            var tsProjectRateHistories = await timesheetApi.GetRateHistories(projectIds, lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'"));
            await _projectRateHistoryService.AddOrUpdateRangeAsync(tsProjectRateHistories, cancellationToken);
            _dataSyncLogger.LogInfo($"{GetType().Name} - Sync completed with number of Rate Histories: {tsProjectRateHistories.Count()}");
        }
    }
}
