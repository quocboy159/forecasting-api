using AutoMapper;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.Domain.Common;
using FSEmployee = ForecastingSystem.Domain.Models.Employee;

namespace ForecastingSystem.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, Domain.Models.Client>()
                .ForMember(d => d.ExternalClientId, opt => opt.MapFrom(s => s.OrganizationID))
                .ForMember(d => d.ClientName, opt => opt.MapFrom(s => s.OrganizationName))
                .ForMember(d => d.ClientType, opt => opt.MapFrom(s => s.ClientTypeName));

            CreateMap<Project, Domain.Models.Project>()
                .ForMember(d => d.ProjectId, opt => opt.Ignore())
                .ForMember(d => d.ProjectName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ProjectCode, opt => opt.MapFrom(s => s.Code))
                .ForMember(d => d.ExternalProjectId, opt => opt.MapFrom(s => s.ProjectId))
                .ForMember(d => d.ProjectType, opt => opt.MapFrom(s => Constants.ProjectType.Project))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(s => s.DueDate))
                .ForMember(d => d.Confident, opt => opt.MapFrom(s => 100))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.ClientId, opt => opt.MapFrom(s => s.FSClientId));

            CreateMap<Phase, Domain.Models.Phase>()
                .ForMember(d => d.PhaseId, opt => opt.Ignore())
                .ForMember(d => d.ExternalPhaseId, opt => opt.MapFrom(s => s.PhaseId))
                .ForMember(d => d.ProjectId, opt => opt.MapFrom(s => s.FSProjectId));

            CreateMap<ProjectRate, Domain.Models.ProjectRate>()
                .ForMember(d => d.ProjectRateId, opt => opt.Ignore())
                .ForMember(d => d.ExternalProjectRateId, opt => opt.MapFrom(s => s.RateId))
                .ForMember(d => d.ProjectId, opt => opt.MapFrom(s => s.FSProjectId));

            CreateMap<BHREmployee , FSEmployee>()
                .ForMember(dest => dest.Email , opt => opt.MapFrom(src => src.WorkEmail))
                .ForMember(dest => dest.ExternalId , opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Dob , opt => opt.MapFrom(src => Utility.BambooHRDateFromString(src.DateOfBirth)))
                .ForMember(dest => dest.DateJoined , opt => opt.MapFrom(src => Utility.BambooHRDateFromString(src.HireDate)))
                // DateLeave is custom map from EmploymentStatus.Terminated date
                .ForMember(dest => dest.DateLeave , opt => opt.Ignore())
                .ForMember(dest => dest.UserName , opt => opt.MapFrom(src => ExtractUserNameFromEmail(src.WorkEmail, src.FirstName, src.LastName)))
                .ForMember(dest => dest.BranchLocation , opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.ActiveStatus,opt => opt.MapFrom( src => true))
                .ForMember(dest => dest.LastSyncDate,opt => opt.MapFrom( src => DateTime.Now))
                .ForMember(dest => dest.Country,opt => opt.MapFrom( src => GetCountryFromJobLocation(src.Location, src.Country)))
                ;

            CreateMap<ProjectRateHistory, Domain.Models.ProjectRateHistory>()
                .ForMember(d => d.ProjectRateHistoryId, opt => opt.Ignore())
                .ForMember(d => d.ExternalRateHistoryId, opt => opt.MapFrom(s => s.RateHistoryId))
                .ForMember(d => d.ProjectRateId, opt => opt.MapFrom(s => s.FSProjectRateId));

            CreateMap<EmployeeLeave, Domain.Models.EmployeeLeave>()
                .ForMember(d => d.TimesheetUsername, opt => opt.MapFrom(s => s.UserName))
                .ForMember(d => d.ExternalLeaveId, opt => opt.MapFrom(s => s.LeaveId))
                .ForMember(d => d.SubmissionDate, opt => opt.MapFrom(s => s.CreatedDate));
           
            CreateMap<BHROption , Domain.Models.Skillset>()
                .ForMember(dest => dest.SkillsetName , opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ExternalId , opt => opt.MapFrom(src => src.Id));
        }

        private string GetCountryFromJobLocation(string jobLocation, string country)
        {
            //Hardcoded for now.
            Dictionary<string , string> JobLocationCountryMap = new Dictionary<string , string>()
            {
                //prod values
                { "HCMC Office","Viet Nam" },
                { "Auckland Office","New Zealand" },
                // test env values
                { "Ho Chi Minh City","Viet Nam" }, 
                { "Auckland","New Zealand" }
            };
            if (string.IsNullOrEmpty(jobLocation)) return country ?? "UnknownJobLocation";
            return JobLocationCountryMap[jobLocation];
        }

        private string? ExtractUserNameFromEmail(string workEmail , string firstName , string lastName)
        {
            if (string.IsNullOrEmpty(workEmail)) return $"{firstName.ToLower()}.{lastName.ToLower()}";
            string userName = workEmail.Remove(workEmail.LastIndexOf("@"));

            return userName;
        }
    }
}
