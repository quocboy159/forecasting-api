using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ForecastingSystem.Domain.Common
{
    public static class ProjectEmployeeManagerExtensions
    {
        public static string GetEmployeeFullName(this IEnumerable<ProjectEmployeeManager> list)
        {
            return string.Join("; ", list?.Select(x => x.Employee.FullName));
        }

        public static int? GetFirstProjectManagerId(this IEnumerable<ProjectEmployeeManager> list)
        {
            return list?.FirstOrDefault()?.EmployeeId;
        }
    }
}
