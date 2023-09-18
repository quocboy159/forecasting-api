using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Models
{
    public class Rate : IAuditable
    {
        public int RateId { get; set; }
        public string RateName { get; set; }
        public float HourlyRate { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
