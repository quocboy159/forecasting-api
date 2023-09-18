namespace ForecastingSystem.DataSyncServices.Outbound

{
    public class BHREmployeeSkill
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string CustomDateAdded { get; set; }
        public string CustomSkill { get; set; }
        public string CustomProficiencyLevel { get; set; }
        public string? CustomNotes1 { get; set; }
    }

}
