using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public string? Description { get; set; }

        public int? DefaultRate { get; set; }


    }
}
