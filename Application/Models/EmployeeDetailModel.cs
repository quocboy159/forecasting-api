using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class EmployeeDetailModel
    {
        public int EmployeeId { get; set; }

        public EmployeeDetailInformationModel Information { get; set; }

        public List<EmployeeDetailSkillsetCategoryModel> SkillsetCategories { get; set; }

        public List<EmployeeDetailLeaveModel> Leaves { get; set; }
    }
}
