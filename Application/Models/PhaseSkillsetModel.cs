using System.Text.Json.Serialization;

namespace ForecastingSystem.Application.Models
{
    public class PhaseSkillsetModel
    {
        public int PhaseSkillSetId { get; set; }
        public int PhaseId { get; set; }
        public int SkillsetId { get; set; }
        public int? Level { get; set; }
    }
}
