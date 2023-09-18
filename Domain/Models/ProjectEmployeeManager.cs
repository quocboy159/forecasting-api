namespace ForecastingSystem.Domain.Models
{
    public class ProjectEmployeeManager: IAuditable
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
    }
}
