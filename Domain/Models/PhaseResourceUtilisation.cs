using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ForecastingSystem.Domain.Models
{
    public class PhaseResourceUtilisation: IAuditable
    {
        public int PhaseResourceUtilisationId { get; set; }
        //public int? EmployeeId { get; set; }
        //public string Username { get; set; }
        //public string Country { get; set; }
        ////public int? ResourcePlaceHolderId { get; set; }
        //public string ResourcePlaceHolderName { get; set; }
        //public int ProjectId { get; set; }
        //public int? ExternalProjectId { get; set; } 
        //public string ProjectName { get; set; }
        public int PhaseId { get; set; }
        //public string PhaseName { get; set; }
        public DateTime StartWeek { get; set; }
        public float TotalHours { get; set; }
        public int PhaseResourceId { get; set; }
        //public double FTE { get; set; }
        public virtual PhaseResource PhaseResource { get; set; }
        //public virtual ICollection<PhaseResourceUtilisationByWeek> PhaseResourceUtilisationByWeeks { get; set; } = new List<PhaseResourceUtilisationByWeek>();
    }
}
