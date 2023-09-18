using System;
using System.Collections.Generic;

namespace ForecastingSystem.Application.Models
{
    public class ProjectListModel
    {
        public IEnumerable<ProjectModel> Projects { get; set; }

        internal object FirstOrDefault(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
