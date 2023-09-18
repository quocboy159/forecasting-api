using System;

namespace ForecastingSystem.Domain.Models
{
    public class Salary {
        public int SalaryId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Salary1 { get; set; }
        public string Currency { get; set; }
        public string SalaryType { get; set; }
        public string PaidPer { get; set; }
        public string Comment { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public int? ExternalId { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
