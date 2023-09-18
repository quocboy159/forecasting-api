using System;

namespace ForecastingSystem.Application.Models
{
    public class ProjectRateModel
    {
        public int ProjectRateId { get; set; }

        public string RateName { get; set; }

        public int ProjectId { get; set; }

        public decimal? Rate { get; set; }
        public DateTime? EffectiveDate { get; set; }

        public int? RoleId { get; set; }

    }
}
