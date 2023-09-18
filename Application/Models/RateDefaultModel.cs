using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class RateModel
    {
        public int RateId { get; set; }
        public string RateName { get; set; }
        public float HourlyRate { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
