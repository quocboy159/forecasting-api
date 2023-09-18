using System;
using System.Collections.Generic;

namespace ForecastingSystem.Application.Models
{
    public class ProjectPhaseModelToAdd
    {
        public int PhaseId { get; set; }

        public string PhaseName { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public decimal? Budget { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public int? TimesheetPhaseId { get; set; }

        public string Status { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsCalculatingByResource { get; set; } = false;

        public IEnumerable<PhaseSkillsetModel> PhaseSkillsets { get; set; }

    }
}
