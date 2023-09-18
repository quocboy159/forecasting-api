using System;
using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{
    public partial class Project: IAuditable
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = null!;

        public int ClientId { get; set; }

        public string? ProjectCode { get; set; }

        public string? Description { get; set; }

        public string? ProjectType { get; set; }
        public decimal? ProjectValue { get; set; }
        public decimal? ProjectBudget { get; set; }
        public bool? IsObsoleteProjectValue { get; set; } = true;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Confident { get; set; }

        public int? ExternalProjectId { get; set; }

        public ProjectStatus Status { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public DateTime? CloseDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public virtual Client Client { get; set; } = null!;

        public virtual ICollection<Phase> Phases { get; set; } = new List<Phase>();

        public virtual ICollection<ProjectRate> ProjectRates { get; set; } = new List<ProjectRate>();

        public virtual ICollection<ProjectEmployeeManager> ProjectEmployeeManagers { get; set; } = new List<ProjectEmployeeManager>();
    }
}