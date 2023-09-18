using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{
    public class SkillsetCategory
    {
        public int SkillsetCategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<Skillset> Skillsets { get; } = new List<Skillset>();
    }
}
