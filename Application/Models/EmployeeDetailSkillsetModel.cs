using ForecastingSystem.Domain.Common;

namespace ForecastingSystem.Application.Models
{
    public class EmployeeDetailSkillsetModel
    {
        public string? ProficiencyLevel { get; set; }

        public int ProficiencyLevelStar => GetProficiencyLevelStar(ProficiencyLevel);

        public string? SkillsetName { get; set; }

        private int GetProficiencyLevelStar(string? proficiencyLevel)
        {
            if (string.IsNullOrEmpty(proficiencyLevel))
            {
                return 0;
            }
            else if (proficiencyLevel.Contains(Constants.SkillsetProficiencyLevel.Beginer))
            {
                return 1;
            }
            else if (proficiencyLevel.Contains(Constants.SkillsetProficiencyLevel.Intermediate))
            {
                return 2;
            }
            else if (proficiencyLevel.Contains(Constants.SkillsetProficiencyLevel.Advanced))
            {
                return 3;
            }
            else if (proficiencyLevel.Contains(Constants.SkillsetProficiencyLevel.Expert))
            {
                return 4;
            }
            else if (proficiencyLevel.Contains(Constants.SkillsetProficiencyLevel.Master))
            {
                return 5;
            }

            return 0;
        }
    }

}
