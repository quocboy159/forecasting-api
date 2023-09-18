using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.DataSyncServices.Outbound

{
    public class EmployeeLeave
    {
        public int LeaveId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Country { get; set; }
        public string DayType { get; set; }
        public float TotalDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LeaveCode { get; set; }
    }
}
