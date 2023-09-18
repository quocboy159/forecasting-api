using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ForecastingSystem.Domain.Models
{
    public partial class Employee : IAuditable
    {
        public int EmployeeId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FullName => $"{FirstName}{(string.IsNullOrEmpty(PreferredName) ? string.Empty : $" ({PreferredName})")}{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}")}{(string.IsNullOrEmpty(LastName) ? string.Empty : $" {LastName}")}".Trim();

        public string? UserName { get; set; }

        public string Gender { get; set; }

        public DateTime? Dob { get; set; }

        public DateTime? DateJoined { get; set; }

        public DateTime? DateLeave { get; set; }

        public string Email { get; set; }
        public string PreferredName { get; set; }
        public string MiddleName { get; set; }
        public bool? ActiveStatus { get; set; }

        public string BranchLocation { get; set; }

        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string Country { get; set; }

        public string WorkingWeeks { get; set; }
        public string WorkingHours { get; set; }
        public string UtilizationRate { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public int? ExternalId { get; set; }

        public virtual ICollection<EmployeeSkillset> EmployeeSkillsets { get; set; } = new List<EmployeeSkillset>();

        public virtual ICollection<PhaseResource> PhaseResources { get; } = new List<PhaseResource>();

        public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

        public virtual ICollection<ProjectEmployeeManager> ProjectEmployeeManagers { get; set; } = new List<ProjectEmployeeManager>();

        public virtual EmployeeUtilisationNotes EmployeeUtilisationNotes { get; set; } = null!;

        [JsonIgnore]
        public int WorkingHoursNumber
        {
            get
            {
                var valid = int.TryParse(WorkingHours, out int hours);
                return valid ? hours : 40;
            }
        }
    }
}
