using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System;

namespace ForecastingSystem.Application.Models
{
    public class ProjectDetailModel
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = null!;

        public string ProjectCode { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; } = null!;

        public string ProjectType { get; set; }

        public string Status { get; set; }

        public int? Confident { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        public List<ProjectDetailRateModel> Rates { get; set; } = new List<ProjectDetailRateModel>();
        public List<ProjectDetailPhaseModel> Phases { get; set; } = new List<ProjectDetailPhaseModel>();
        public float ProjectValue { get; set; }
        public bool? IsObsoleteProjectValue { get; set; }
        public IList<int> ProjectManagerIds { get; set; }
    }
}
