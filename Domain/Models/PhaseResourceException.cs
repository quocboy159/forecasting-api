using System;

namespace ForecastingSystem.Domain.Models
{
    public class PhaseResourceException: IAuditable
    {
        public int PhaseResourceExceptionId { get; set; }
        public int PhaseResourceId { get; set; }      

        public DateTime StartWeek { get; set; }

        public int NumberOfWeeks { get; set; }

        public int HoursPerWeek { get; set; }

        public virtual PhaseResource PhaseResource { get; set; }

    }
}
