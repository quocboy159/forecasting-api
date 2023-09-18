namespace ForecastingSystem.Application.Models
{
    public class PhaseResourceModelToView: PhaseResourceModel
    {
        public int ProjectId { get; set; }
        public string ProjectRateName { get; set; }
        public string? NameWithRate { get; set;} = null;
        public string? EmployeeBranchLocation { get; set; } = null;
    }
}
