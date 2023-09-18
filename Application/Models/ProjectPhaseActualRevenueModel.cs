using ForecastingSystem.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ForecastingSystem.Application.Models
{
    public class ProjectPhaseActualRevenueModel
    {
        public int ExternalProjectId { get; set; }
        public string ProjectName { get; set; }
        public string PhaseCode { get; set; }
        public List<PhaseRevenueDetailModel> RevenueByWeeks { get; set; } = new List<PhaseRevenueDetailModel>();
        public List<PhaseRevenueDetailModel> RevenueByMonths { get; set; } = new List<PhaseRevenueDetailModel>();

    }
}
