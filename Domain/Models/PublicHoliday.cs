using System;

namespace ForecastingSystem.Domain.Models
{
    public partial class PublicHoliday
    {
        public int PublicHolidayId { get; set; }

        public string? Name { get; set; }

        public DateTime? Date { get; set; }

        public string? Country { get; set; }

        public int? ExternalLeaveHolidayId { get; set; }
    }
}