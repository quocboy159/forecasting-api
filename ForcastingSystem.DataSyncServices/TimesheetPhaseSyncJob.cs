using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class TimesheetPhaseSyncJob : BaseSyncJob
    {
        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly IPhaseService _phaseService;
        private readonly IProjectService _projectService;
        public TimesheetPhaseSyncJob(ITimesheetConnectionFactory timesheetConnectionFactory
            , IPhaseService phaseService
            , IProjectService projectService
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService)
            : base(dataSyncLogger, datetimeHelper, dataSyncProcessService)
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _phaseService = phaseService;
            _projectService = projectService;
        }

        public override string DataSyncType => DataSyncTypes.TimesheetSyncPhaseJob;

        public override string Source => DataSyncSources.TimesheetSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.TimesheetSyncPhaseJobOrder;

        protected override async Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            List<int> externalProjectIds = await _projectService.GetSyncedProjectExternalIds();
            var projectIds = string.Join<int>("," , externalProjectIds);

            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            var tsPhases = await timesheetApi.GetPhases(projectIds, lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'"));
            await _phaseService.AddOrUpdateRangeAsync(tsPhases, cancellationToken);
            _dataSyncLogger.LogInfo($"{GetType().Name} - Sync completed with number of Phases: {tsPhases.Count()}");
        }
    }
}
