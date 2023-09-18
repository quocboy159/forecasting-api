using System.Collections.Generic;
using System;

namespace ForecastingSystem.Domain.Models
{
    public class AuditEntry
    {
        public long AuditEntryId { get; set; }
        public string EntityName { get; set; }
        public string ActionType { get; set; }
        public string Username { get; set; }
        public DateTime TimeStamp { get; set; }
        public string EntityId { get; set; }
        public Dictionary<string , object> Changes { get; set; }
    }
}
