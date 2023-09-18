using System;

namespace ForecastingSystem.Application.Models
{
    public class ProjectDetailRateModel
    {
        public int ProjectRateId { get; set; }

        public string RateName { get; set; }

        public decimal? HourlyRate { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}
