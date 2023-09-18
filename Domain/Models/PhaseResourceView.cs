using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ForecastingSystem.Domain.Models
{
    public class PhaseResourceView
    {

        public int PhaseResourceUtilisationId { get; set; }
        public int? EmployeeId { get; set; }
        public string Username { get; set; }
        public string Country { get; set; }
        public int? ResourcePlaceHolderId { get; set; }
        public string ResourcePlaceHolderName { get; set; }
        public int ProjectId { get; set; }
        public int? ExternalProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string? ProjectType { get; set; }
        public string ClientName { get; set; }
        public int PhaseId { get; set; }
        public string PhaseName { get; set; }
        public DateTime StartWeek { get; set; }
        public float TotalHours { get; set; }
        public int PhaseResourceId { get; set; }
        public double FTE { get; set; }
        public List<PhaseResourceUtilisation> PhaseResourceUtilisations { get; set; }
    }
}
