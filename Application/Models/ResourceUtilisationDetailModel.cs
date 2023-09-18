using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ForecastingSystem.Application.Models
{
    public class ResourceUtilisationDetailModel
    {
        public int WorkingHours { get; private set; }
        public DateTime StartDate { get; set; }
        public float Hours { get; set; }
        public double Percentage
        {
            get
            {
                var percentageValue = (double)(Hours * 100 / Math.Max(WorkingHours, 1));

                return Math.Round(percentageValue, 2, MidpointRounding.AwayFromZero) ;
            }
        }
        public bool IsActual { get; internal set; }

        public ResourceUtilisationDetailModel SetWorkingHours(int workingHours)
        {
            this.WorkingHours = workingHours;
            return this;
        }
    }
}
