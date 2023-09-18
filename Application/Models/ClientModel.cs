using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;

namespace ForecastingSystem.Application.Models
{
    public class ClientModel
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public string ClientType { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public virtual ICollection<Project> Projects { get; } = new List<Project>();
    }
}
