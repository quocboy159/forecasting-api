namespace ForecastingSystem.Application.Models
{
    public class PhaseResourceExceptionModelToView : PhaseResourceExceptionModel
    {
        public string NameWithRate { get; set; }
        public int PhaseId { get; set; }
    }
}
