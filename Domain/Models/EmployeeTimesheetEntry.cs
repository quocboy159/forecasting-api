using System;

namespace ForecastingSystem.Domain.Models
{
    public class EmployeeTimesheetEntry
    {
        public int EmployeeTimesheetEntryId { get; set; }
        public int ExternalTimesheetId { get; set; }
        public string TimesheetUsername { get; set; }
        public int ExternalProjectId { get; set; }
        public string ProjectName { get; set; }
        public string PhaseCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Hours { get; set; }
        public string RateName { get; set; }
        public double? RateAmount { get; set; }
        public int? ExternalRateId { get; set; }

    }
}
