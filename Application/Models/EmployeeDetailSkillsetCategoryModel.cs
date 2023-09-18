using System.Collections.Generic;

namespace ForecastingSystem.Application.Models
{
    public class EmployeeDetailSkillsetCategoryModel
    {
        public string CategoryName { get; set; } = null!;

        public List<EmployeeDetailSkillsetModel> Skillsets { get; set; }
    }

}
