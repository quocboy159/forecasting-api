using System;
using System.Text.Json.Serialization;

namespace ForecastingSystem.Application.Models
{
    public class PhaseResourceModel
    {
        public int PhaseResourceId { get; set; }

        public int? EmployeeId { get; set; } = null;

        public string FullName { get; set; }

        public int PhaseId { get; set; }

        public int ProjectRateId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double HoursPerWeek { get; set; }

        public double FTE { get; set; }

        public int? ResourcePlaceHolderId { get; set; } = null;
        public string Country { get; set; }

    }
}
