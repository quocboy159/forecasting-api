using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.CustomExceptions
{
    public class ProjectPhaseRevenueException : Exception
    {
        public ProjectPhaseRevenueException()
        {
        }

        public ProjectPhaseRevenueException(string message)
            : base(message)
        {
        }

        public ProjectPhaseRevenueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
