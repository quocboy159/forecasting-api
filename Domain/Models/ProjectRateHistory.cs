using System;

namespace ForecastingSystem.Domain.Models
{
    public partial class ProjectRateHistory : IAuditable
    {
        public int ProjectRateHistoryId { get; set; }

        public int ProjectRateId { get; set; }

        public int ExternalRateHistoryId { get; set; }

        public double? Rate { get; set; }

        public DateTime? StartDate { get; set; }

        public virtual ProjectRate RateNavigation { get; set; } = null!;
    }
}
