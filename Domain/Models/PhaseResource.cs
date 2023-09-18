using System;
using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{
    public class PhaseResource : IAuditable
    {

        public int PhaseResourceId { get; set; }

        public int? EmployeeId { get; set; }

        public int PhaseId { get; set; }

        public int ProjectRateId { get; set; }

        public double HoursPerWeek { get; set; }

        public double FTE { get; set; }

        public int? ResourcePlaceHolderId { get; set; }

        public virtual Employee? Employee { get; set; }

        public virtual Phase Phase { get; set; }

        public virtual ProjectRate ProjectRate { get; set; }

        public virtual ICollection<PhaseResourceException> PhaseResourceExceptions { get; set; } = new List<PhaseResourceException>();

        public virtual ResourcePlaceHolder? ResourcePlaceHolder { get; set; }
        public virtual ICollection<PhaseResourceUtilisation> PhaseResourceUtilisations { get; set; } = new List<PhaseResourceUtilisation>();

    }
}
