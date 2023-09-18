namespace ForecastingSystem.DataSyncServices.Outbound

{
    public class BHRCompensationItem
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string StartDate { get; set; }
        public BHRSalaryRate Rate { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public string PaidPer { get; set; }
        public string PaySchedule { get; set; }
        public BHRSalaryRate OvertimeRate { get; set; }
    }

}
