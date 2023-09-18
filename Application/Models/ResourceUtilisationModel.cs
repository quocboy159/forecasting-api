using ForecastingSystem.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class ResourceUtilisationModel
    {
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int WorkingHours { get; set; }
        public int ResourcePlaceHolderId { get; set; }
        public List<ProjectResourceUtilisationModel> ProjectResourceUtilisations { get; set; } = new List<ProjectResourceUtilisationModel>();
        public EmployeeUtilisationNotesModel Notes { get; set; } 
         [JsonIgnore]
        public DateTime? LargestWeek
        {
            get
            {
                return ProjectResourceUtilisations != null && ProjectResourceUtilisations.Any() ? ProjectResourceUtilisations.Max(s => s.LargestWeek) : null;
            }
        }
        public List<ResourceUtilisationDetailModel> UtilisationByWeeks
        {
            get
            {
                var result = new List<ResourceUtilisationDetailModel>();
                if(SmallesttWeek == null || LargestWeek == null)
                {
                    return result;
                }

                if (ProjectResourceUtilisations != null && ProjectResourceUtilisations.Any())
                {
                    var startDate = SmallesttWeek.Value;
                    var endDate = LargestWeek.Value;
                    while (startDate <= endDate)
                    {
                        var totalHours = 0f;
                        var isActual = false;
                        ProjectResourceUtilisations.ForEach(s =>
                        {
                            if (s.UtilisationByWeeksDictionary != null && s.UtilisationByWeeksDictionary.ContainsKey(startDate))
                            {
                                totalHours += s.UtilisationByWeeksDictionary[startDate].Hours;
                                isActual = s.UtilisationByWeeksDictionary[startDate].IsActual;
                            }
                        });
                        var model = new ResourceUtilisationDetailModel()
                        {
                            StartDate = startDate,
                            Hours = totalHours,
                            IsActual = isActual,

                        };
                        model.SetWorkingHours(WorkingHours);
                        result.Add(model);
                        startDate = startDate.NextWeekMonday();
                    }
                }
                return result;
            }
        }
        [JsonIgnore]
        public DateTime? SmallesttWeek
        {
            get
            {
                return ProjectResourceUtilisations != null && ProjectResourceUtilisations.Any() ? ProjectResourceUtilisations.Min(s => s.SmallestWeek) : null;
            }
        }
    }
}
