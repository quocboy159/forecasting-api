namespace ForecastingSystem.Domain.Models
{
    public class PhaseSkillset: IAuditable
    {
        public int PhaseSkillSetId { get; set; }

        public int PhaseId { get; set; }

        public int SkillsetId { get; set; }

        public int? Level { get; set; }

        public virtual Phase Phase { get; set; } = null!;

        public virtual Skillset Skillset { get; set; } = null!;
    }
}
