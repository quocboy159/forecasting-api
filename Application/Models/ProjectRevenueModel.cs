using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class ProjectRevenueModel
    {
        public int ProjectId { get; set; }
        public int? ExternalProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string ProjectManagerName { get; set; }
        public int? Confident { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ProjectType { get; set; }
        public List<PhaseRevenueModel> PhaseRevenues { get; set; } = new List<PhaseRevenueModel>(); 
        public float ProjectValue
        {
            get
            {
                if (PhaseRevenues != null)
                {
                    return PhaseRevenues.Sum(s => s.PhaseValue);
                }
                return 0f;
            }
        }
        public bool HasChangedProjectValue { get; set; } = false;
        public string[] Errors
        {
            get
            {
                if (PhaseRevenues != null)
                {
                    return PhaseRevenues.Select(s => s.Error).ToArray();
                }
                return null;
            }
        }
        [JsonIgnore]
        public DateTime? LargestWeek
        {
            get
            {
                return PhaseRevenues != null && PhaseRevenues.Any() ? PhaseRevenues.Max(s => s.LargestWeek) : null;
            }
        }

        [JsonIgnore]
        public DateTime? LargestMonth
        {
            get
            {
                return PhaseRevenues != null && PhaseRevenues.Any() ? PhaseRevenues.Max(s => s.LargestMonth) : null;
            }
        }
    }
}
