using System;

namespace ForecastingSystem.Domain.Models
{
    public class EmployeeLeave
    {
        public int EmployeeLeaveId { get; set; }
        public string TimesheetUsername { get; set; }
        public string Status { get; set; }
        public float TotalDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayType { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string LeaveCode { get; set; }
        public int ExternalLeaveId { get; set; }
    }
}
