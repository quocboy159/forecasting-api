namespace ForecastingSystem.Application.Models
{
    public class SkillsetModel
    {
        public int SkillsetId { get; set; }

        public int SkillsetCategoryId { get; set; }

        public string SkillsetCategoryName { get; set; }

        public string SkillsetName { get; set; }

        public string Description { get; set; }

        public string SkillsetTypeId { get; set; }
    }
}
