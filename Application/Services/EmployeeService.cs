using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeLeaveRepository _employeeLeaveRepository;
        private readonly IEmployeeUtilisationNotesRepository _employeeUtilisationNotesRepository;
        private readonly IDefaultResourcePlaceHolderRepository _defaultResourcePlaceHolderRepository;
        private readonly IUserIdLookupRepository _userIdLookupRepository;
        private readonly IPublicHolidayRepository _publicHolidayRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository,
            IProjectRateRepository projectRateRepository,
            IEmployeeLeaveRepository employeeLeaveRepository,
            IEmployeeUtilisationNotesRepository employeeUtilisationNotesRepository,
            IDefaultResourcePlaceHolderRepository defaultResourcePlaceHolderRepository,
            IUserIdLookupRepository userIdLookupRepository,
            IPublicHolidayRepository publicHolidayRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _employeeLeaveRepository = employeeLeaveRepository;
            _employeeUtilisationNotesRepository = employeeUtilisationNotesRepository;
            _defaultResourcePlaceHolderRepository = defaultResourcePlaceHolderRepository;
            _userIdLookupRepository = userIdLookupRepository;
            _publicHolidayRepository = publicHolidayRepository;
        }

        public EmployeeListModel GetEmployees()
        {
            var employees = _employeeRepository.GetAllAsync().Result;
            var employeeListViewModel = _mapper.Map<IEnumerable<EmployeeModel>>(employees);

            return new EmployeeListModel()
            {
                Employees = employeeListViewModel
            };
        }

        public async Task<IEnumerable<EmployeeModel>> GetProjectResourcesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var models = _mapper.Map<List<EmployeeModel>>(employees);
            var defaultResourePlaceHolders = await _defaultResourcePlaceHolderRepository.GetAllAsync();
            models.AddRange(_mapper.Map<List<EmployeeModel>>(defaultResourePlaceHolders));
            return models;
        }

        public async Task<bool> IsExistingEmployeeAsync(int employeeId)
        {
            return await _employeeRepository.ExistAsync(employeeId);
        }

        public async Task<EmployeeDetailModel> GetEmployeeDetailAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeDetailAsync(employeeId);

            var employeeLeaves = new List<EmployeeLeave>();

            var userIdLookup = await _userIdLookupRepository.GetByBambooEmailAsync(employee.Email);
            if (userIdLookup != null)
            {
                employeeLeaves = await _employeeLeaveRepository.GetLeavesLastSevenMonthBy(userIdLookup.TimesheetUserName);
            }

            var models = new EmployeeDetailModel();

            models.EmployeeId = employeeId;

            models.Information = new EmployeeDetailInformationModel()
            {
                Country = employee.Country,
                FirstName = employee.FirstName,
                FullName = employee.FullName,
                JobTitle = employee.JobTitle,
                LastName = employee.LastName,
                ForecastWarning = employee.EmployeeUtilisationNotes?.ForecastWarning,
                InternalWorkNotes = employee.EmployeeUtilisationNotes?.InternalWorkNotes,
                OtherNotes = employee.EmployeeUtilisationNotes?.OtherNotes
            };

            models.SkillsetCategories = employee.EmployeeSkillsets.GroupBy(x => new { x.Skillset.SkillsetCategory.CategoryName }).Select(x =>
                new EmployeeDetailSkillsetCategoryModel
                {
                    CategoryName = x.Key.CategoryName,
                    Skillsets = x.Select(t => new EmployeeDetailSkillsetModel
                    {
                        ProficiencyLevel = t.ProficiencyLevel,
                        SkillsetName = t.Skillset.SkillsetName
                    }).ToList()
                }).ToList();

            models.Leaves = employeeLeaves.Select(x => new EmployeeDetailLeaveModel()
            {
               DayType = x.DayType,
               EndDate = x.EndDate,
               LeaveCode = x.LeaveCode,
               StartDate = x.StartDate,
               Status = x.Status,
               SubmissionDate = x.SubmissionDate,
               TotalDays = x.TotalDays 
            }).ToList();

            return models;
        }

        public async Task SaveEmployeeUtilisationNotes(EmployeeUtilisationNotesModel model)
        {
            var entity = await _employeeUtilisationNotesRepository.FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);

            if (entity == null)
            {
                entity = new EmployeeUtilisationNotes
                {
                    EmployeeId = model.EmployeeId,
                    ForecastWarning = model.ForecastWarning,
                    InternalWorkNotes = model.InternalWorkNotes,
                    OtherNotes = model.OtherNotes,
                };

                await _employeeUtilisationNotesRepository.AddAsync(entity);
            }
            else
            {
                entity.ForecastWarning = model.ForecastWarning;
                entity.InternalWorkNotes = model.InternalWorkNotes;
                entity.OtherNotes = model.OtherNotes;

                await _employeeUtilisationNotesRepository.UpdateAsync(entity);
            }

            await _employeeUtilisationNotesRepository.SaveChangesAsync();
        }        
    }
}
