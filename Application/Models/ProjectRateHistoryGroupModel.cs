using System.Collections.Generic;

namespace ForecastingSystem.Application.Models
{
    public class ProjectRateHistoryGroupModel
    {
        public int ProjectRateId { get; set; }

        public string? RateName { get; set; }

        public List<ProjectRateHistoryModel> ProjectRateHistories { get; set; }
    }
}
