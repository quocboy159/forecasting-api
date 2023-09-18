using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.CustomExceptions;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using static ForecastingSystem.Domain.Common.Constants;
using FSEmployee = ForecastingSystem.Domain.Models.Employee;

namespace ForecastingSystem.DataSyncServices
{
    public class BambooHREmployeeDetailsSyncJob : BaseSyncJob
    {
        private readonly IBambooHRConnectionFactory _bambooHRConnectionFactory;
        private readonly IEmployeePersistentService _employeeService;
        private readonly ISyncSkillsetCategoryRepository _skillsetCategoryRepository;
        private readonly IMapper _mapper;
        public BambooHREmployeeDetailsSyncJob(IBambooHRConnectionFactory bambooHRConnectionFactory
            , IDataSyncServiceLogger dataSyncLogger
            , DatetimeHelper datetimeHelper
            , IDataSyncProcessService dataSyncProcessService
            , IMapper mapper
            , IEmployeePersistentService employeeService
            , ISyncSkillsetCategoryRepository skillsetCategoryRepository
            )
            : base(dataSyncLogger , datetimeHelper , dataSyncProcessService)
        {
            _bambooHRConnectionFactory = bambooHRConnectionFactory;
            _mapper = mapper;
            _employeeService = employeeService;
            _skillsetCategoryRepository = skillsetCategoryRepository;
        }

        public override string DataSyncType => DataSyncTypes.BambooHRSyncEmployeeDetailJob;

        public override string Source => DataSyncSources.BambooHRSource;

        public override string Target => DataSyncTargets.ForecastTarget;

        public override int SyncOrder => SyncOrders.BambooHRSyncEmployeeDetailJobOrder;

        protected override Task SyncData(CancellationToken cancellationToken , DateTime lastSyncTime)
        {
            // Do we need this log?
            _dataSyncLogger.LogInfo($"{GetType().Name} - SyncData started with lastSyncTime {lastSyncTime}");
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
            IBambooHRApi bambooHRApi = _bambooHRConnectionFactory.GetBambooHrHttpClient();

            try
            {
                BHRLastChangedEmployees employees = GetChangedEmloyeesFromBambooHr(ref lastSyncTime , bambooHRApi);

                if (employees.Employees != null && employees.Employees.Any())
                {
                    _dataSyncLogger.LogInfo($"There are {employees.Employees.Count} employees to be sync.");

                    foreach (var item in employees.Employees)
                    {
                        var employeeId = item.Key;
                        if (item.Value.Action == BambooHREmployeeLastChanged.Deleted)
                        {
                            _dataSyncLogger.LogInfo($"Syncing found Deleted employee ExternalId {employeeId}, Setting that employee inactive if ExternalId found in db.");
                            _employeeService.SetEmployeeInactive(employeeId);
                            continue;
                        }
                        //GET Single Employee
                        var bhrEmployee = bambooHRApi.GetEmployeeById(employeeId).Result;


                        if (bhrEmployee.EmploymentStatus == BambooHRAccountStatus.Inactive)
                        {
                            //Delete if externalId exists, otherwise skip inactive employees 
                            FSEmployee inactiveFsEmployee = _mapper.Map<FSEmployee>(bhrEmployee);
                            inactiveFsEmployee.ActiveStatus = false;
                            _dataSyncLogger.LogInfo($"Syncing Inactive employee {inactiveFsEmployee.FullName}...");
                            _employeeService.SetEmployeeInactive(inactiveFsEmployee);
                            continue;
                        }
                        FSEmployee fsEmployee = _mapper.Map<FSEmployee>(bhrEmployee);
                        _dataSyncLogger.LogInfo($"Syncing employee {fsEmployee.FullName}...");

                        //employee skills
                        var skills = bambooHRApi.GetEmployeeSkills(employeeId).Result;
                        AppendSkillsetToEmployee(fsEmployee , skills);

                        //compensation - salary
                        var compensations = bambooHRApi.GetEmployeeCompensationInfo(employeeId).Result;
                        AppendSalaryInfo(fsEmployee , compensations);

                        //employment status => LeaveDate
                        var employmentStatus = bambooHRApi.GetEmployeeEmploymentStatus(employeeId).Result;
                        var termination = employmentStatus.FirstOrDefault(x => x.EmploymentStatus == BambooHREmploymentStatusValues.Terminated);
                        if (termination != null)
                        {
                            fsEmployee.DateLeave = Utility.BambooHRDateFromString(termination.Date);
                        }

                        //customAdditionalCompensationInfo => workingHours, workingweeks, utilisationRate
                        var additionalCompensationInfo = bambooHRApi.GetEmployeeAdditionalCompensationInfo(employeeId).Result;
                        AppendAdditionalCompensationInfo(fsEmployee , additionalCompensationInfo);

                        try
                        {
                            _employeeService.SaveEmployee(fsEmployee);
                            _dataSyncLogger.LogInfo($"Syncing employee {fsEmployee.FullName} Successfully.");
                        }
                        catch (Exception syncEmployeeException)
                        {
                            var options = new JsonSerializerOptions
                            {
                                ReferenceHandler = ReferenceHandler.Preserve ,
                                WriteIndented = true
                            };

                            string employeeJsonString = JsonSerializer.Serialize(fsEmployee , options);

                            _dataSyncLogger.LogInfo($"Debuging data for employee {fsEmployee.FullName}:{employeeJsonString}");
                            _dataSyncLogger.LogError($"Syncing employee {fsEmployee.FullName} Failed - {syncEmployeeException.Message}" , syncEmployeeException);

                            throw;
                        }
                    }
                }
                else
                {
                    _dataSyncLogger.LogInfo("There is no employee to be updated");
                }

            }
            catch (Exception ex)
            {
                _dataSyncLogger.LogError($"{GetType().Name} - SyncJob failed at - {ex.Message}" , ex);
                throw;
            }
            return Task.CompletedTask;
        }


        private static BHRLastChangedEmployees GetChangedEmloyeesFromBambooHr(ref DateTime lastSyncTime , IBambooHRApi bambooHRApi)
        {
            string BambooHRDateFormat = "yyyy-MM-ddTHH:mm:ss'Z'";
            if (lastSyncTime != DateTime.MinValue)
            {
                lastSyncTime = lastSyncTime.AddHours(-DateTimeOffset.Now.Offset.Hours);
            }
            string sinceDateTime = lastSyncTime.ToString(BambooHRDateFormat);
            var employees = bambooHRApi.GetChangedEmployees(sinceDateTime).Result;
            return employees;
        }

        private static void AppendSalaryInfo(FSEmployee fsEmployee , BHRCompensationItem[] compensations)
        {
            if (compensations != null && compensations.Any())
            {
                fsEmployee.Salaries = new List<Salary>();
                foreach (var compensation in compensations)
                {
                    var salary = new Salary()
                    {
                        ExternalId = Utility.ToInt(compensation.Id) ,
                        StartDate = Utility.BambooHRDateFromString(compensation.StartDate) ,
                        Salary1 = compensation.Rate.Value ,
                        Currency = compensation.Rate.Currency ,
                        PaidPer = compensation.PaidPer ,
                        Comment = compensation.Comment ,
                        SalaryType = compensation.Type ,
                        LastSyncDate = DateTime.Now
                    };
                    fsEmployee.Salaries.Add(salary);
                }
            }
        }

        private void AppendSkillsetToEmployee(FSEmployee fsEmployee , BHREmployeeSkill[] skills)
        {
            if (skills != null && skills.Any())
            {
                int skillsetCategoryId = _skillsetCategoryRepository.CreateSkillsetCategoryIfNotExist(SkillsetCategories.Default);

                fsEmployee.EmployeeSkillsets = new List<EmployeeSkillset>();
                foreach (var skill in skills)
                {
                    var skillset = new EmployeeSkillset()
                    {
                        Skillset = new Skillset
                        {
                            SkillsetName = skill.CustomSkill ,
                            SkillsetCategoryId = skillsetCategoryId
                        } ,
                        Note = skill.CustomNotes1 ,
                        ProficiencyLevel = skill.CustomProficiencyLevel ,
                        StartDate = Utility.BambooHRDateFromString(skill.CustomDateAdded) ,
                        ExternalId = skill.Id ,
                        //TODO: deal with inActive Employee later
                        ActiveStatus = true
                    };

                    fsEmployee.EmployeeSkillsets.Add(skillset);
                }
            }
        }


        private static void AppendAdditionalCompensationInfo(FSEmployee fsEmployee , BHREmployeeAdditionalCompensation[] additionalCompensationInfo)
        {
            if (additionalCompensationInfo != null && additionalCompensationInfo.Any())
            {
                //If many items found, the one with biggest Id is the latest update from BambooHR
                var info = additionalCompensationInfo.OrderByDescending(x => x.Id).FirstOrDefault();
                fsEmployee.WorkingHours = info?.CustomWorkingHours;
                fsEmployee.WorkingWeeks = info?.CustomWorkingWeeks;
                fsEmployee.UtilizationRate = info?.CustomUtilizationRate;
            }
        }

    }
}
