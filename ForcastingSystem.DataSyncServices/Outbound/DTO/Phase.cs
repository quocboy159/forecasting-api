namespace ForecastingSystem.DataSyncServices.Outbound
{
    public class Phase
    {
        public int PhaseId { get; set; }
        public string PhaseName { get; set; }
        public string PhaseCode { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public double? Budget { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int FSProjectId { get; set; }
    }
}
