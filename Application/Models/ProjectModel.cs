using System;

namespace ForecastingSystem.Application.Models
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = null!;

        public int ClientId { get; set; }

        public string ProjectCode { get; set; }

        public string Description { get; set; }

        public string ProjectType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public float ProjectValue { get; set; }

        public decimal? ProjectBudget { get; set; }

        public int? Confident { get; set; }

        public bool? ActiveStatus { get; set; }

        public int? ExternalId { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public string ClientName { get; set; } = null!;

    }
}
