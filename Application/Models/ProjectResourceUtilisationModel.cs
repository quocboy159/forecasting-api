using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class ProjectResourceUtilisationModel
    {
        public int? ExternalProjectId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string? ProjectType { get; set; }
        public string ClientName { get; set; }
        public bool IsPMProject { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsPlaceHolder { get; set; }
        public int? ResourcePlaceHolderId { get; set; }
        public float? ResourcePlaceHolder_FTE { get; set; }
        public string ResourcePlaceHolder_PhaseName { get; set; }
        public List<ResourceUtilisationDetailModel> UtilisationByWeeks { get; set; } = new List<ResourceUtilisationDetailModel>();
        [JsonIgnore]
        public DateTime? LargestWeek
        {
            get
            {
                return UtilisationByWeeks != null && UtilisationByWeeks.Any() ? UtilisationByWeeks.Last().StartDate : null;
            }
        }
        [JsonIgnore]
        public DateTime? SmallestWeek
        {
            get
            {
                return UtilisationByWeeks != null && UtilisationByWeeks.Any() ? UtilisationByWeeks.First().StartDate : null;
            }
        }
        [JsonIgnore]
        public Dictionary<DateTime, ResourceUtilisationDetailModel> UtilisationByWeeksDictionary
        {
            get
            {
                return UtilisationByWeeks != null && UtilisationByWeeks.Any() ?
                    UtilisationByWeeks.ToDictionary(s => s.StartDate) : null;
            }
        }
    }
}
