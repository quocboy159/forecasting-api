using System;

namespace ForecastingSystem.Application.Models
{
    public class EmployeeDetailLeaveModel
    {
        public string Status { get; set; }
        public float TotalDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayType { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string LeaveCode { get; set; }
    }

}
