using FluentAssertions;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Application.Tests
{
    public class ActualProjectRevenueCalculatorTest
    {
        private readonly IProjectRevenueCalculatorService _calculatorService;   
        public ActualProjectRevenueCalculatorTest(IProjectRevenueCalculatorService calculatorService) 
        {
            _calculatorService = calculatorService;
        }
        private List<EmployeeTimesheetEntry> TC1_CreateTimesheetEntry()
        {
            var entries = new List<EmployeeTimesheetEntry>() { 
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 1,
                    ExternalTimesheetId= 1,
                    ExternalProjectId= 1,
                    ProjectName = "Skillet",
                    PhaseCode = "Phase 01",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 06, 8, 0, 0),
                    EndDate= new DateTime(2023, 03, 06, 16, 0, 0),
                    Hours = 8
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 2,
                    ExternalTimesheetId= 2,
                    ExternalProjectId= 1,
                    ProjectName = "Skillet",
                    PhaseCode = "Phase 01",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 07, 8, 0, 0),
                    EndDate= new DateTime(2023, 03, 07, 16, 0, 0),
                    Hours = 8
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 3,
                    ExternalTimesheetId= 3,
                    ExternalProjectId= 1,
                    ProjectName = "Skillet",
                    PhaseCode = "Phase 01",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 08, 8, 0, 0),
                    EndDate= new DateTime(2023, 03, 08, 16, 0, 0),
                    Hours = 8
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 4,
                    ExternalTimesheetId= 4,
                    ExternalProjectId= 1,
                    ProjectName = "Skillet",
                    PhaseCode = "Phase 02",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 09, 8, 0, 0),
                    EndDate= new DateTime(2023, 03, 09, 16, 0, 0),
                    Hours = 8
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 5,
                    ExternalTimesheetId= 5,
                    ExternalProjectId= 1,
                    ProjectName = "Skillet",
                    PhaseCode = "Phase 02",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 10, 8, 0, 0),
                    EndDate= new DateTime(2023, 03, 10, 16, 0, 0),
                    Hours = 8
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 7,
                    ExternalTimesheetId= 7,
                    ExternalProjectId= 2,
                    ProjectName = "Medtech",
                    PhaseCode = "01 Enhance",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 13, 8, 0, 0),
                    EndDate= new DateTime(2023, 03, 13, 12, 0, 0),
                    Hours = 4
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 8,
                    ExternalTimesheetId= 8,
                    ExternalProjectId= 2,
                    ProjectName = "Medtech",
                    PhaseCode = "01 Enhance",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 14, 8, 0, 0),
                    EndDate= new DateTime(2023, 03, 14, 12, 0, 0),
                    Hours = 4
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 9,
                    ExternalTimesheetId= 9,
                    ExternalProjectId= 2,
                    ProjectName = "Medtech",
                    PhaseCode = "01 Enhance",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 15, 8, 0, 0),
                    EndDate= new DateTime(2023, 03, 15, 12, 0, 0),
                    Hours = 4
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 10,
                    ExternalTimesheetId= 10,
                    ExternalProjectId= 3,
                    ProjectName = "CDG",
                    PhaseCode = "04 Hotfix",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 13, 12, 0, 0),
                    EndDate= new DateTime(2023, 03, 13, 16, 0, 0),
                    Hours = 4
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 11,
                    ExternalTimesheetId= 11,
                    ExternalProjectId= 3,
                    ProjectName = "CDG",
                    PhaseCode = "04 Hotfix",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 14, 12, 0, 0),
                    EndDate= new DateTime(2023, 03, 14, 16, 0, 0),
                    Hours = 4
                },
                new EmployeeTimesheetEntry()
                {
                    EmployeeTimesheetEntryId= 12,
                    ExternalTimesheetId= 12,
                    ExternalProjectId= 3,
                    ProjectName = "CDG",
                    PhaseCode = "04 Hotfix",
                    ExternalRateId = 1,
                    RateName = "VN Dev/Test",
                    RateAmount = 80,
                    TimesheetUsername = "tho.vu",
                    StartDate= new DateTime(2023, 03, 15, 12, 0, 0),
                    EndDate= new DateTime(2023, 03, 15, 16, 0, 0),
                    Hours = 4
                }
            };
            
            return entries;
        }
        
        [Fact]
        public void TC1_Calculate_ActualProjectRevenue_Success()
        {
            var entries = TC1_CreateTimesheetEntry();
            var startWeek = new DateTime(2023, 03, 06);
            var thisWeek = new DateTime(2023, 03, 27);
            var projectPhases = _calculatorService.GetActualProjectRevenues(entries, startWeek, thisWeek);

            projectPhases.Should().NotBeNull();
            projectPhases.Count.Should().Be(4);

            var skilletPhase01 = projectPhases.ElementAt(0);
            skilletPhase01.ExternalProjectId.Should().Be(1);
            skilletPhase01.ProjectName.Should().BeSameAs("Skillet");
            skilletPhase01.PhaseCode.Should().BeSameAs("Phase 01");
            var weekRevenues = skilletPhase01.RevenueByWeeks.ToDictionary(s => s.StartDate, x => x.Revenue);
            var expectedWeekRevenues = new Dictionary<DateTime, float>
            {
                { new DateTime(2023, 03, 06), 1920 },
            };
            weekRevenues.Should().BeEquivalentTo(expectedWeekRevenues);

            var skilletPhase02 = projectPhases.ElementAt(1);
            skilletPhase02.ExternalProjectId.Should().Be(1);
            skilletPhase02.ProjectName.Should().BeSameAs("Skillet");
            skilletPhase02.PhaseCode.Should().BeSameAs("Phase 02");
            weekRevenues = skilletPhase02.RevenueByWeeks.ToDictionary(s => s.StartDate, x => x.Revenue);
            expectedWeekRevenues = new Dictionary<DateTime, float>
            {
                { new DateTime(2023, 03, 06), 1280 },
            };
            weekRevenues.Should().BeEquivalentTo(expectedWeekRevenues);

            var medtechPhase01 = projectPhases.ElementAt(2);
            medtechPhase01.ExternalProjectId.Should().Be(2);
            medtechPhase01.ProjectName.Should().BeSameAs("Medtech");
            medtechPhase01.PhaseCode.Should().BeSameAs("01 Enhance");
            weekRevenues = medtechPhase01.RevenueByWeeks.ToDictionary(s => s.StartDate, x => x.Revenue);
            expectedWeekRevenues = new Dictionary<DateTime, float>
            {
                { new DateTime(2023, 03, 13), 960 },
            };
            weekRevenues.Should().BeEquivalentTo(expectedWeekRevenues);

            var cdgPhase04 = projectPhases.ElementAt(3);
            cdgPhase04.ExternalProjectId.Should().Be(3);
            cdgPhase04.ProjectName.Should().BeSameAs("CDG");
            cdgPhase04.PhaseCode.Should().BeSameAs("04 Hotfix");
            weekRevenues = cdgPhase04.RevenueByWeeks.ToDictionary(s => s.StartDate, x => x.Revenue);
            expectedWeekRevenues = new Dictionary<DateTime, float>
            {
                { new DateTime(2023, 03, 13), 960 },
            };
            weekRevenues.Should().BeEquivalentTo(expectedWeekRevenues);
        }
    }
}