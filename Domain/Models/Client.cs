using System;
using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{
    public partial class Client : IAuditable
    {
        public int ClientId { get; set; }

        public string? ClientName { get; set; }

        public string? ClientType { get; set; }

        public string? ClientCode { get; set; }

        public int? ExternalClientId { get; set; }

        public virtual ICollection<Project> Projects { get; } = new List<Project>();
    }

}