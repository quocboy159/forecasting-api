using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    public class TimesheetEntrySyncJob : BaseSyncJob
    {
        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly ITimesheetEntryService _timesheetEntryService;
        private readonly IProjectService _projectService;

        public TimesheetEntrySyncJob(ITimesheetConnectionFactory timesheetConnectionFactory
            , ITimesheetEntryService timesheetEntryService
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService
            , IProjectService projectService)
            : base(dataSyncLogger, datetimeHelper, dataSyncProcessService)
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _timesheetEntryService = timesheetEntryService;
            _projectService = projectService;
        }

        public override string DataSyncType => DataSyncTypes.TimesheetSyncEntryJob;

        public override string Source => DataSyncSources.TimesheetSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.TimesheetSyncEntryJobOrder;

        protected override async Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            List<int> externalProjectIds = await _projectService.GetSyncedProjectExternalIds();

            var syncTime = lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'");
            var batches = externalProjectIds.Batch(20);
            int totalEntries = 0;
            int totalBatches = 0;
            int totalAPICalls = 0;
            foreach (var batch in batches)
            {
                totalBatches++;
                int pageSize = 5000; int pageNumber = 0;
                var projectIdsGroup = string.Join<int>("," , batch);
                int numberOfReturnedRecords = 0;

                do
                {
                    totalAPICalls++;
                    pageNumber++;
                    var tsEntries = await timesheetApi.GetTimesheetEntries(projectIdsGroup , pageSize , pageNumber , syncTime);
                    numberOfReturnedRecords = tsEntries.Count();
                    totalEntries += numberOfReturnedRecords;
                    await _timesheetEntryService.AddOrUpdateRangeAsync(tsEntries , cancellationToken);

                } while (numberOfReturnedRecords == pageSize);

                int baaa = 0;

            }

            string totalMsg = $"totalBatches: {totalBatches}, total timesheet entries synced :{totalEntries}, total timesheet API calls {totalAPICalls}";
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData Ended, {totalMsg}");
        }    }
}

