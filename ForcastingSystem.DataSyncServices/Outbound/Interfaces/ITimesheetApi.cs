using Refit;

namespace ForecastingSystem.DataSyncServices.Outbound
{
    public interface ITimesheetApi
    {
        [Get("/api/Forecast/GetClients?lastSyncDate={lastSyncDate}")]
        Task<IEnumerable<Client>> GetClients(string lastSyncDate = "null");

        [Get("/api/Forecast/GetProjects?lastSyncDate={lastSyncDate}")]
        Task<Project[]> GetProjects(string lastSyncDate = "null");

        [Get("/api/Forecast/v2/GetPhases?projectIds={projectIdsStr}&lastSyncDate={lastSyncDate}")]
        Task<Phase[]> GetPhases(string projectIdsStr, string lastSyncDate = "null");

        [Get("/api/Forecast/v2/GetRates?projectIds={projectIdsStr}&lastSyncDate={lastSyncDate}")]
        Task<ProjectRate[]> GetRates(string projectIdsStr , string lastSyncDate = "null");

        [Get("/api/Forecast/v2/GetRateHistories?projectIds={projectIdsStr}&lastSyncDate={lastSyncDate}")]
        Task<ProjectRateHistory[]> GetRateHistories(string projectIdsStr , string lastSyncDate = "null");

        [Get("/api/Forecast/GetTimesheetEntriesByProjects?projectIds={projectIdsStr}&lastSyncDate={lastSyncDate}&pageSize={pageSize}&pageNumber={pageNumber}")]
        Task<TimesheetEntry[]> GetTimesheetEntries( string projectIdsStr, int pageSize , int pageNumber, string lastSyncDate = "null");

        [Get("/api/Forecast/GetInactiveTimesheetEntries?lastSyncDate={lastSyncDate}")]
        Task<TimesheetDeletedEntry[]> GetTimesheetDeletedEntries(string lastSyncDate = "null");

        [Get("/api/Forecast/GetPublicHolidays?lastSyncDate={lastSyncDate}")]
        Task<PublicHoliday[]> GetPublicHolidays(string lastSyncDate = "null");

        [Get("/api/Forecast/GetEmployeeLeaves?lastSyncDate={lastSyncDate}")]
        Task<IEnumerable<EmployeeLeave>> GetEmployeeLeaves(string lastSyncDate = "null");

        [Get("/api/Forecast/GetEmployees?lastSyncDate={lastSyncDate}")]
        Task<IEnumerable<Employee>> GetEmployees(string lastSyncDate = "null");
    }
}
