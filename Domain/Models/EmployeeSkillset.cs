using System;

namespace ForecastingSystem.Domain.Models
{
    public class EmployeeSkillset
    {
        public int EmployeeSkillsetId { get; set; }

        public int SkillsetId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ProficiencyLevel { get; set; }

        public string? Note { get; set; }

        public bool? ActiveStatus { get; set; }

        public int? ExternalId { get; set; }

        public virtual Employee Employee { get; set; } = null!;

        public virtual Skillset Skillset { get; set; } = null!;
    }
}
