using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class TimesheetProjectRateSyncJob: BaseSyncJob
    {
        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly IProjectRateService _projectRateService;
        private readonly IProjectService _projectService;
        public TimesheetProjectRateSyncJob(ITimesheetConnectionFactory timesheetConnectionFactory
            , IProjectRateService projectRateService
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService
            , IProjectService projectService
            )
            : base(dataSyncLogger , datetimeHelper , dataSyncProcessService)
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _projectRateService = projectRateService;
            _projectService = projectService;
        }

        public override string DataSyncType => DataSyncTypes.TimesheetSyncProjectRateJob;

        public override string Source => DataSyncSources.TimesheetSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.TimesheetSyncProjectRateJobOrder;

        protected override async Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
            List<int> externalProjectIds = await _projectService.GetSyncedProjectExternalIds();
            var projectIds = string.Join<int>("," , externalProjectIds);

            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            var tsProjectRates = await timesheetApi.GetRates(projectIds, lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'"));
            await _projectRateService.AddOrUpdateRangeAsync(tsProjectRates, cancellationToken);
            _dataSyncLogger.LogInfo($"{GetType().Name} - Sync completed with number of Rates: {tsProjectRates.Count()}");
        }
    }
}
