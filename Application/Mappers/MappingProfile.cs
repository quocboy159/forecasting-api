using AutoMapper;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ForecastingSystem.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<ProjectRate, PhaseResourceModel>().ReverseMap();
            CreateMap<Client, ClientModel>().ReverseMap();
            CreateMap<Project, ProjectModel>().ReverseMap();
            CreateMap<Phase, ProjectPhaseModel>().ReverseMap()
                .ForMember(d => d.PhaseSkillsets, opt => opt.Ignore());
            CreateMap<Employee, EmployeeModel>();
            CreateMap<PhaseResourceExceptionModel, PhaseResourceException>();
            CreateMap<PhaseResourceException, PhaseResourceExceptionModelToView>()
                .ForMember(d => d.NameWithRate, opt => opt.MapFrom(s => $"{s.PhaseResource.Employee.FullName} - {s.PhaseResource.ProjectRate.RateName}"))
                .ForMember(d => d.PhaseId, opt => opt.MapFrom(s => s.PhaseResource.PhaseId));

            CreateMap<PhaseResourceModel, PhaseResource>()
                .ForMember(d => d.EmployeeId, opt => opt.Condition(s => string.IsNullOrWhiteSpace(s.FullName) || s.FullName.ToLower().IndexOf("(place holder)") < 0))
                .ForMember(d => d.ResourcePlaceHolder, opt => opt.MapFrom(s => MapToResourcePlaceHolder(s)));
            CreateMap<PhaseResource, PhaseResourceModel>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.EmployeeId > 0 ? s.Employee.FullName : s.ResourcePlaceHolder.ResourcePlaceHolderName));
            CreateMap<PhaseResource, PhaseResourceModelToView>()
                .ForMember(d => d.ProjectId, opt => opt.MapFrom(s => s.Phase.ProjectId))
                .ForMember(d => d.ProjectRateName, opt => opt.MapFrom(s => s.ProjectRate.RateName))
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.EmployeeId > 0 ? s.Employee.FullName : s.ResourcePlaceHolder.ResourcePlaceHolderName))
                .ForMember(d => d.Country, opt => opt.MapFrom(s => s.EmployeeId > 0 ? s.Employee.Country : s.ResourcePlaceHolder.Country));

            CreateMap<Phase, ProjectDetailPhaseModel>()
                .ForMember(d => d.PhaseCode, opt => opt.MapFrom(s => s.PhaseCode))
                .ForMember(d => d.PhaseBudget, opt => opt.MapFrom(s => s.Budget))
                .ForMember(d => d.EstimatedEndDate, opt => opt.MapFrom(s => s.EstimatedEndDate))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(s => s.EndDate))
                .ReverseMap();

            CreateMap<ProjectRate, ProjectDetailRateModel>()
                .ForMember(d => d.ProjectRateId, opt => opt.MapFrom(s => s.ProjectRateId))
                .ReverseMap();

            CreateMap<Project, ProjectDetailModel>()
                .ForMember(d => d.ClientName, opt => opt.MapFrom(s => s.Client.ClientName))
                .ForMember(d => d.Rates, opt => opt.MapFrom(s => s.ProjectRates))
                .ForMember(d => d.ProjectManagerIds, opt => opt.MapFrom(s => s.ProjectEmployeeManagers.Select(x => x.EmployeeId)))
                .ReverseMap();

            CreateMap<Project, ProjectDetailAddEditModel>()
                .ForMember(d => d.ClientName, opt => opt.MapFrom(s => s.Client.ClientName))
                .ForMember(d => d.Rates, opt => opt.MapFrom(s => s.ProjectRates))
                .ReverseMap();

            CreateMap<SkillsetAddModel, SkillsetModel>();

            CreateMap<Skillset, SkillsetModel>()
                .ForMember(d => d.SkillsetCategoryName, opt => opt.MapFrom(s => s.SkillsetCategory.CategoryName)).ReverseMap();
            CreateMap<PhaseSkillset, PhaseSkillsetModelToView>()
                .ForMember(d => d.SkillsetName, opt => opt.MapFrom(s => s.Skillset.SkillsetName));
            CreateMap<PhaseSkillsetModelToView, PhaseSkillset>();
            CreateMap<PhaseSkillsetModel, PhaseSkillset>();
            CreateMap<ProjectPhaseModelToAdd, ProjectPhaseModel>()
                .ForMember(d => d.PhaseSkillsets, opt => opt.MapFrom(s => MapToPhaseSkillsetModelToView(s.PhaseSkillsets)));

            CreateMap<ProjectRate, EmployeeModel>()
                .ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => $"PH{s.ProjectRateId}"))
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => $"{s.RateName} (Place holder)"))
                .ForMember(d => d.ActiveStatus, opt => opt.MapFrom(s => true))
                .ForAllOtherMembers(d => d.Ignore());

            CreateMap<ProjectRate, ProjectRateModel>();

            CreateMap<PublicHoliday, PublicHolidayModel>();
            CreateMap<EmployeeUtilisationNotesAddEditModel, EmployeeUtilisationNotesModel>();
            CreateMap<EmployeeUtilisationNotes, EmployeeUtilisationNotesModel>();

            CreateMap<ProjectRateHistory, ProjectRateHistoryModel>();
            CreateMap<ProjectRate, ProjectRateHistoryGroupModel>();
            CreateMap<DefaultResourcePlaceHolder, DefaultResourcePlaceHolderModel>();
            CreateMap<DefaultResourcePlaceHolder, EmployeeModel>()
                .ForMember(d => d.EmployeeId, opt => opt.MapFrom(s => $"PH{s.DefaultResourcePlaceHolderId}"))
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ActiveStatus, opt => opt.MapFrom(s => true))
                .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Country))
                .ForMember(d => d.WorkingHours, opt => opt.MapFrom(s => "40"))
                .ForMember(d => d.WorkingHoursNumber, opt => opt.MapFrom(s => 40))
                .ForAllOtherMembers(d => d.Ignore());

            CreateMap<UserIdLookup, UserIdLookupModel>();
            CreateMap<DataSyncProcess, DataSyncProcessModel>();

        }


        private IEnumerable<PhaseSkillsetModelToView> MapToPhaseSkillsetModelToView(IEnumerable<PhaseSkillsetModel> src)
        {
            return src.Select(x => new PhaseSkillsetModelToView()
            {
                PhaseSkillSetId = x.PhaseSkillSetId,
                PhaseId = x.PhaseId,
                SkillsetId = x.SkillsetId,
                Level = x.Level
            });
        }

        private ResourcePlaceHolder MapToResourcePlaceHolder(PhaseResourceModel model)
        {
            if (!string.IsNullOrEmpty(model.FullName) && model.FullName.ToLower().IndexOf("(place holder)") > 0)
                return new ResourcePlaceHolder { ResourcePlaceHolderName = model.FullName, Country = model.Country };
            return null;
        }
    }
}
