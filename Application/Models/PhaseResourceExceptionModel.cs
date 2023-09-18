using System;
using System.Text.Json.Serialization;

namespace ForecastingSystem.Application.Models
{
    public class PhaseResourceExceptionModel
    {
        public int PhaseResourceExceptionId { get; set; }

        public int PhaseResourceId { get; set; }

        public DateTime StartWeek { get; set; }

        public int NumberOfWeeks { get; set; }

        public int HoursPerWeek { get; set; }

    }
}
