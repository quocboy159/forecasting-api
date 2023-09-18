using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System;

namespace ForecastingSystem.Application.Models
{
    public class ProjectDetailAddEditModel
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

        public IList<int> ProjectManagerIds { get; set; } = new List<int>();

        public string Description { get; set; }

        public List<ProjectDetailRateModel> Rates { get; set; } = new List<ProjectDetailRateModel>();
        public List<ProjectDetailPhaseModel> Phases { get; set; } = new List<ProjectDetailPhaseModel>();
    }
}
