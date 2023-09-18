using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices
{
    /// <summary>
    /// Leave System share the same Web API with Timesheet
    /// </summary>
    public class LeaveSystemSyncJob : BaseSyncJob
    {

        private readonly ITimesheetConnectionFactory _timesheetConnectionFactory;
        private readonly IMapper _mapper;

        public LeaveSystemSyncJob(ITimesheetConnectionFactory timesheetConnectionFactory, IMapper mapper
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService)
            : base (dataSyncLogger, datetimeHelper , dataSyncProcessService) 
        {
            _timesheetConnectionFactory = timesheetConnectionFactory;
            _mapper = mapper;
        }

        public override string DataSyncType => DataSyncTypes.LeaveSystemSyncJob;

        public override string Source => DataSyncSources.LeaveSystemSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.LeaveSystemSyncJobOrder;

        protected override Task SyncData(CancellationToken cancellationToken, DateTime lastSyncTime)
        {
            // Do we need this log?
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
            ITimesheetApi timesheetApi = _timesheetConnectionFactory.GetTimesheetHttpClient();
            var holidays = timesheetApi.GetPublicHolidays(lastSyncTime == DateTime.MinValue ? "null" : lastSyncTime.ToString()).Result;
            var numberOfHolidays = holidays.Length;
            //TODO: Implement the sync logic here

            return Task.CompletedTask;
        }
    }
}
