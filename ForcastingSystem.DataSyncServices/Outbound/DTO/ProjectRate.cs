namespace ForecastingSystem.DataSyncServices.Outbound
{
    public class ProjectRate
    {
        public int RateId { get; set; }
        public string RateName { get; set; }
        public string Notes { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool IsVisibleInTimesheet { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int FSProjectId { get; set; }
    }
}
