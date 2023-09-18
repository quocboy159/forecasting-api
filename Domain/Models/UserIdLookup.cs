using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Models
{
    public class UserIdLookup
    {
        public int Id { get; set; }

        public string BambooHRFirstName { get; set; }

        public string BambooHRLastName { get; set; }

        public string BambooHREmail { get; set; }

        public string TimesheetUserName { get; set; }

        public string TimesheetEmail { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }
    }
}
