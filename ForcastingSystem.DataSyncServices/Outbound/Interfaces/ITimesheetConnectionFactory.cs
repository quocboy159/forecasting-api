namespace ForecastingSystem.DataSyncServices.Outbound.Interfaces
{
    public interface ITimesheetConnectionFactory
    {
        ITimesheetApi GetTimesheetHttpClient();
    }
}
