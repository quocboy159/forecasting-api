namespace ForecastingSystem.DataSyncServices.Outbound

{
    public class BHREmployeeAdditionalCompensation
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string CustomCostperhour { get; set; }
        public string CustomWorkingWeeks { get; set; }
        public string CustomWorkingHours { get; set; }
        public string CustomUtilizationRate { get; set; }
    }

}
