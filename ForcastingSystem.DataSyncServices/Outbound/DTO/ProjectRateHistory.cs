namespace ForecastingSystem.DataSyncServices.Outbound
{
    public class ProjectRateHistory
    {
        public int RateHistoryId { get; set; }
        public int RateId { get; set; }
        public string RateName { get; set; }
        public double? Rate { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int FSProjectRateId { get; set; }
    }
}
