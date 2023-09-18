using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using DbAuditEntry = ForecastingSystem.Domain.Models.AuditEntry;
namespace ForecastingSystem.Infrastructure.Data.Context
{
    public class AuditEntry: DbAuditEntry
    {
        //Keep this property in Data Layer not in Domain as it has dependency on EFCore
        public List<PropertyEntry> TempProperties { get; set; }
    }
}
