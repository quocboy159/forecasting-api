using FluentAssertions;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Application.Tests
{
    public class ProjectRevenueCalculatorTest
    {
        private readonly IProjectRevenueCalculatorService _calculatorService;   
        public ProjectRevenueCalculatorTest(IProjectRevenueCalculatorService calculatorService) 
        {
            _calculatorService = calculatorService;
        }
        private List<Project> TC1_CreateProjects()
        {
            var project = new Project()
            {
                ProjectId = 1,
                Phases = new List<Phase>()
                {
                    new Phase()
                    {
                        PhaseId= 1,
                        StartDate = new DateTime(2022, 12, 19),
                        Budget = 82000,
                        PhaseResources = new List<PhaseResource>()
                        {
                            new PhaseResource()
                            {
                                PhaseId= 1,
                                EmployeeId= 1,
                                Employee = new Employee()
                                {
                                    EmployeeId= 1,
                                    UserName = "kent.huang",
                                    Country = "NZ",
                                    Email = "kent.huang@codehq.nz"
                                },
                                FTE = 0.5,
                                HoursPerWeek = 20,
                                ProjectRateId= 1,
                                ProjectRate =new ProjectRate()
                                {
                                    ProjectRateId= 1,
                                    RateName = "Project Manager",
                                    ProjectRateHistories = new List<ProjectRateHistory>()
                                    {
                                        new ProjectRateHistory()
                                        {
                                            ProjectRateId= 1,
                                            ProjectRateHistoryId= 1,
                                            Rate = 180,
                                            StartDate= new DateTime(2022, 12, 01)
                                        }
                                    }
                                },
                                PhaseResourceExceptions = new List<PhaseResourceException>()
                                {
                                    new PhaseResourceException()
                                    {
                                        StartWeek= new DateTime(2022, 12, 26),
                                        HoursPerWeek= 8,
                                        NumberOfWeeks = 1,
                                        PhaseResourceId = 1
                                    }
                                }
                            },
                            new PhaseResource()
                            {
                                PhaseId= 1,
                                EmployeeId= 2,
                                Employee = new Employee()
                                {
                                    EmployeeId= 2,
                                    UserName = "tho.vu1",
                                    Country = "VN",
                                    Email = "tho.vu1@codehq.nz"
                                },
                                FTE = 1,
                                HoursPerWeek = 40,
                                ProjectRateId= 2,
                                ProjectRate =new ProjectRate()
                                {
                                    ProjectRateId= 2,
                                    RateName = "VN dev",
                                    ProjectRateHistories = new List<ProjectRateHistory>()
                                    {
                                        new ProjectRateHistory()
                                        {
                                            ProjectRateId= 2,
                                            ProjectRateHistoryId= 2,
                                            Rate = 80,
                                            StartDate= new DateTime(2022, 12, 01)
                                        },
                                        new ProjectRateHistory()
                                        {
                                            ProjectRateId= 2,
                                            ProjectRateHistoryId= 3,
                                            Rate = 90,
                                            StartDate= new DateTime(2023, 1, 01)
                                        },
                                        new ProjectRateHistory()
                                        {
                                            ProjectRateId= 2,
                                            ProjectRateHistoryId= 4,
                                            Rate = 100,
                                            StartDate= new DateTime(2023, 1, 15)
                                        }
                                    }
                                }
                            },
                            new PhaseResource()
                            {
                                PhaseId= 1,
                                ResourcePlaceHolderId= 1,
                                ResourcePlaceHolder = new ResourcePlaceHolder()
                                {
                                    ResourcePlaceHolderId= 1,
                                    ResourcePlaceHolderName = "VN Dev (placeholder)",
                                    Country = "Viet Nam"
                                },
                                FTE = 2,
                                HoursPerWeek = 80,
                                ProjectRateId= 2,
                                ProjectRate =new ProjectRate()
                                {
                                    ProjectRateId= 2,
                                    RateName = "VN dev",
                                    ProjectRateHistories = new List<ProjectRateHistory>()
                                    {
                                        new ProjectRateHistory()
                                        {
                                            ProjectRateId= 2,
                                            ProjectRateHistoryId= 2,
                                            Rate = 80,
                                            StartDate= new DateTime(2022, 12, 01)
                                        },
                                        new ProjectRateHistory()
                                        {
                                            ProjectRateId= 2,
                                            ProjectRateHistoryId= 3,
                                            Rate = 90,
                                            StartDate= new DateTime(2023, 1, 01)
                                        },
                                        new ProjectRateHistory()
                                        {
                                            ProjectRateId= 2,
                                            ProjectRateHistoryId= 4,
                                            Rate = 100,
                                            StartDate= new DateTime(2023, 1, 15)
                                        }
                                    }
                                }
                            }
                        }  
                    }
                }
            };
            return new List<Project>() { project };
        }
        private List<PublicHoliday> TC1_CreatePublicHolidays()
        {
            return new List<PublicHoliday>()
            { 
                new PublicHoliday()
                {
                    Country = "NZ",
                    Date= new DateTime(2022,12,26)
                },
                new PublicHoliday()
                {
                    Country = "NZ",
                    Date= new DateTime(2022,12,27)
                },
                new PublicHoliday()
                {
                    Country = "NZ",
                    Date= new DateTime(2023,1,2)
                },
                new PublicHoliday()
                {
                    Country = "NZ",
                    Date= new DateTime(2023,1,3)
                },
                new PublicHoliday()
                {
                    Country = "VN",
                    Date= new DateTime(2023,1,10)
                },
                new PublicHoliday()
                {
                    Country = "VN",
                    Date= new DateTime(2023,1,12)
                }
            };
        }
        private List<EmployeeLeave> TC1_CreateEmployeeLeave()
        {
            return new List<EmployeeLeave>()
            {
                new EmployeeLeave()
                {
                    EmployeeLeaveId= 1,
                    TimesheetUsername = "tho.vu",
                    Status = "Approved",
                    TotalDays = 2,
                    StartDate = new DateTime(2023,01,19),
                    EndDate = new DateTime(2023,01,20),
                    DayType = "Whole days"
                },
                new EmployeeLeave()
                {
                    EmployeeLeaveId= 3,
                    TimesheetUsername = "tho.vu",
                    Status = "Approved",
                    TotalDays = 0.5f,
                    StartDate = new DateTime(2023,01,23),
                    EndDate = new DateTime(2023,01,23),
                    DayType = "Haft days"
                },
                new EmployeeLeave()
                {
                    EmployeeLeaveId = 2,
                    TimesheetUsername = "kent.huang",
                    Status = "Approved",
                    TotalDays = 1,
                    StartDate = new DateTime(2023,01,4),
                    EndDate = new DateTime(2023,01,4),
                    DayType = "Whole days"
                }
            };
        }
        private List<UserIdLookup> TC1_CreateUserIdsLookup()
        {
            return new List<UserIdLookup>() 
            {
                new UserIdLookup() 
                {
                    Id= 1,
                    BambooHREmail = "tho.vu1@codehq.nz",
                    TimesheetUserName = "tho.vu"
                },
                new UserIdLookup()
                {
                    Id = 2,
                    BambooHREmail = "kent.huang@codehq.nz",
                    TimesheetUserName = "kent.huang"
                }
            };
        }
        [Fact]
        public void TC1_Calculate_ProjectRevenue_Success_With_2Resources_1Exception()
        {
            var project = TC1_CreateProjects();
            var holidays = TC1_CreatePublicHolidays();
            var leaves = TC1_CreateEmployeeLeave();
            var userIdsLookup = TC1_CreateUserIdsLookup();
            var result = _calculatorService.GetProjectRevenue(project.First(), holidays, leaves, userIdsLookup);

            result.Should().NotBeNull();
            result.PhaseRevenues.Count.Should().Be(1);
            result.PhaseRevenues.First().RevenueByWeeks.Count.Should().Be(7);
            var weekRevenues = result.PhaseRevenues.First().RevenueByWeeks.ToDictionary(s => s.StartDate, x => x.Revenue);
            var expectedWeekRevenues = new Dictionary<DateTime, float>
            {
                { new DateTime(2022, 12, 19), 13200 },
                { new DateTime(2022, 12, 26), 10464 },
                { new DateTime(2023, 1, 2), 12240 },
                { new DateTime(2023, 1, 9), 10080 },
                { new DateTime(2023, 1, 16), 14000 },
                { new DateTime(2023, 1, 23), 14800 },
                { new DateTime(2023, 1, 30), 6240 }
            };
            weekRevenues.Should().BeEquivalentTo(expectedWeekRevenues);
            result.PhaseRevenues.First().PhaseValue.Should().Be(81024);
            result.ProjectValue.Should().Be(81024);

            var weekImpactStatDays = result.PhaseRevenues.First().RevenueByWeeks.ToDictionary(s => s.StartDate, x => x.ImpactStatDays);
            var expectedWeekImpactStatDays = new Dictionary<DateTime, float>
            {
                { new DateTime(2022, 12, 19), 0 },
                { new DateTime(2022, 12, 26), 1440 },
                { new DateTime(2023, 1, 2), 1440 },
                { new DateTime(2023, 1, 9), 4320 },
                { new DateTime(2023, 1, 16), 0 },
                { new DateTime(2023, 1, 23), 0 },
                { new DateTime(2023, 1, 30), 0 }
            };
            weekImpactStatDays.Should().BeEquivalentTo(expectedWeekImpactStatDays);

            var weekImpactLeaveDays = result.PhaseRevenues.First().RevenueByWeeks.ToDictionary(s => s.StartDate, x => x.ImpactLeave);
            var expectedWeekImpactLeaveDays = new Dictionary<DateTime, float>
            {
                { new DateTime(2022, 12, 19), 0 },
                { new DateTime(2022, 12, 26), 0 },
                { new DateTime(2023, 1, 2), 720 },
                { new DateTime(2023, 1, 9), 0 },
                { new DateTime(2023, 1, 16), 1600 },
                { new DateTime(2023, 1, 23), 800 },
                { new DateTime(2023, 1, 30), 0 }
            };
            weekImpactLeaveDays.Should().BeEquivalentTo(expectedWeekImpactLeaveDays);

            result.PhaseRevenues.First().RevenueByMonths.Count.Should().Be(2);
            var monthRevenues = result.PhaseRevenues.First().RevenueByMonths.ToDictionary(s => s.StartDate, x => x.Revenue);
            var expectedMonthRevenues = new Dictionary<DateTime, float>
            {
                { new DateTime(2022, 12, 1), 23664 },
                { new DateTime(2023, 1, 1), 57360 },
            };
            monthRevenues.Should().BeEquivalentTo(expectedMonthRevenues);

            var monthImpactStatDays = result.PhaseRevenues.First().RevenueByMonths.ToDictionary(s => s.StartDate, x => x.ImpactStatDays);
            var expectedMonthImpactStatDays = new Dictionary<DateTime, float>
            {
                { new DateTime(2022, 12, 1), 1440 },
                { new DateTime(2023, 1, 1), 5760 },
            };
            monthImpactStatDays.Should().BeEquivalentTo(expectedMonthImpactStatDays);

            var monthImpactLeaveDays = result.PhaseRevenues.First().RevenueByMonths.ToDictionary(s => s.StartDate, x => x.ImpactLeave);
            var expectedMonthImpactLeaveDays = new Dictionary<DateTime, float>
            {
                  { new DateTime(2022, 12, 1), 0 },
                { new DateTime(2023, 1, 1), 3120 },
            };
            monthImpactLeaveDays.Should().BeEquivalentTo(expectedMonthImpactLeaveDays);
        }
    }
}