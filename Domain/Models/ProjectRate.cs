using System;
using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{

    public partial class ProjectRate: IAuditable
    {
        public int ProjectRateId { get; set; }

        public string? RateName { get; set; }

        public int ProjectId { get; set; }
        
        public int? ExternalProjectRateId { get; set; }

        public ProjectRateStatus Status { get; set; }

        public virtual ICollection<PhaseResource> PhaseResources { get; } = new List<PhaseResource>();

        public virtual Project Project { get; set; } = null!;

        public virtual ICollection<ProjectRateHistory> ProjectRateHistories { get; set; } = new List<ProjectRateHistory>();

    }
}