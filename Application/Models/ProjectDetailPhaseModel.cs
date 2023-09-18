using System;

namespace ForecastingSystem.Application.Models
{
    public class ProjectDetailPhaseModel
    {
        public int PhaseId { get; set; }

        public string PhaseName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? PhaseBudget { get; set; }

        public string PhaseCode { get; set; }

        public string Status { get; set; }

        public DateTime? EstimatedEndDate { get; set; }

        public bool IsCompleted { get; set; }
        public bool? IsCalculatingByResource { get; set; } = false;
    }
}
