using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{
    public class Skillset {
        public int SkillsetId { get; set; }

        public int SkillsetCategoryId { get; set; }

        public string? SkillsetName { get; set; }

        public string? Description { get; set; }

        public string? SkillsetTypeId { get; set; }

        public int ExternalId { get; set; }

        public virtual ICollection<EmployeeSkillset> EmployeeSkillsets { get; } = new List<EmployeeSkillset>();

        public virtual ICollection<PhaseSkillset> PhaseSkillsets { get; } = new List<PhaseSkillset>();

        public virtual SkillsetCategory SkillsetCategory { get; set; } = null!;
    }
}
