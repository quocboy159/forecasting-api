using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using ForecastingSystem.Domain.Models;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class TimesheetProjectSyncJob : BaseSyncJob
    {
        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly IProjectService _projectService;
        public TimesheetProjectSyncJob(ITimesheetConnectionFactory timesheetConnectionFactory 
            , IProjectService projectService
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService)
            : base(dataSyncLogger, datetimeHelper, dataSyncProcessService)
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _projectService = projectService;
        }

        public override string DataSyncType => DataSyncTypes.TimesheetSyncProjectJob;

        public override string Source => DataSyncSources.TimesheetSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.TimesheetSyncProjectJobOrder;

        protected override async Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            var timesheetProjects = await timesheetApi.GetProjects(lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'"));
            //filter out: closed date, inactive projects, Augen projects
            var filteredProjects = timesheetProjects.Where(x => 
            x.CloseDate == new DateTime(1900 , 01 , 01)
            && x.Status == ProjectStatus.Active.ToString()
            && ! x.Code.StartsWith("200")
            && !x.Code.StartsWith("100100")
            ).ToList();          

            var numberOfProjects = filteredProjects.Count();

            await _projectService.AddOrUpdateRangeAsync(filteredProjects , cancellationToken);
        }
    }
}
