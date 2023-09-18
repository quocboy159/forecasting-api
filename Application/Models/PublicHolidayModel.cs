using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class PublicHolidayModel
    {
        public int PublicHolidayId { get; set; }

        public string? Name { get; set; }

        public DateTime? Date { get; set; }

        public string? Country { get; set; }
    }
}
