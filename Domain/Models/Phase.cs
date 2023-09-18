using System;
using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{

    public class Phase
    {
        public int PhaseId { get; set; }

        public string PhaseName { get; set; }
        public string PhaseCode { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public decimal? Budget { get; set; }
        public decimal? PhaseValue { get; set; }

        public PhaseStatus Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }

        public int? TimesheetPhaseId { get; set; }

        public bool IsCompleted { get; set; }
        public bool? IsCalculatingByResource { get; set; } = false;

        public int? ExternalPhaseId { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<PhaseResource> PhaseResources { get; set; } = new List<PhaseResource>();      

        public virtual ICollection<PhaseSkillset> PhaseSkillsets { get; set; } = new List<PhaseSkillset>();

    }
}
