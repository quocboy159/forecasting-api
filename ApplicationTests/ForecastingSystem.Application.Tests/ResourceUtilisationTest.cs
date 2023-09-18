using AutoMapper;
using FluentAssertions;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Application.Services;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using Moq;

namespace ForecastingSystem.Application.Tests
{
    public class ResourceUtilisationTest
    {
        private const int _external_skillet_id = 1;
        private const int _external_medtech_id = 2;
        private readonly ResourceUtilisationService _resourceUtilisationService;
        public ResourceUtilisationTest()
        {
            //_resourceUtilisationService = resourceUtilisationService;
            var timesheetEntryRepoMock = new Mock<IEmployeeTimesheetEntryRepository>();
            var phaseResourceRepoMock = new Mock<IPhaseResourceRepository>();
            var userIdLookupRepoMock = new Mock<IUserIdLookupRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();
            var employeeUtilisationNotesRepoMock = new Mock<IEmployeeUtilisationNotesRepository>();
            var mapper = new Mock<IMapper>();
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            var projectRepoMock = new Mock<IProjectRepository>();

            timesheetEntryRepoMock.Setup(repo => repo.GetEntries(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                            .Returns(Task.FromResult(CreateActualTimesheetEntries()));
            phaseResourceRepoMock.Setup(repo => repo.GetPhaseResourceUtilisations(It.IsAny<DateTime>()))
                                            .Returns(Task.FromResult(CreatePhaseResourceUtilisation()));
            userIdLookupRepoMock.Setup(repo => repo.GetAllAsync())
                                            .Returns(Task.FromResult(CreateUserIdLookup().AsEnumerable()));
            employeeRepoMock.Setup(repo => repo.GetEmployeesForResourceUtilisationAsync())
                                            .Returns(Task.FromResult(CreateEmployees()));
            employeeUtilisationNotesRepoMock.Setup(repo => repo.GetAllAsync())
                                            .Returns(Task.FromResult((new List<EmployeeUtilisationNotes>()).AsEnumerable()));

            _resourceUtilisationService = new ResourceUtilisationService(mapper.Object, timesheetEntryRepoMock.Object,
                phaseResourceRepoMock.Object, userIdLookupRepoMock.Object,
                employeeRepoMock.Object, employeeUtilisationNotesRepoMock.Object,
                projectRepoMock.Object, currentUserServiceMock.Object);
        }

        private List<EmployeeTimesheetEntry> CreateActualTimesheetEntries()
        {
            var employeeTimesheetEntryId = 1;
            var externalTimesheetId = 1;

            var result = new List<EmployeeTimesheetEntry>();
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_skillet_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Skillet",
                PhaseCode = "Phase 01",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 8,
                StartDate = new DateTime(2023, 04, 17, 08, 00, 00),
                EndDate = new DateTime(2023, 04, 17, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_skillet_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Skillet",
                PhaseCode = "Phase 01",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 8,
                StartDate = new DateTime(2023, 04, 18, 08, 00, 00),
                EndDate = new DateTime(2023, 04, 18, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_skillet_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Skillet",
                PhaseCode = "Phase 01",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 8,
                StartDate = new DateTime(2023, 04, 19, 08, 00, 00),
                EndDate = new DateTime(2023, 04, 19, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_skillet_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Skillet",
                PhaseCode = "Phase 01",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 4,
                StartDate = new DateTime(2023, 04, 20, 08, 00, 00),
                EndDate = new DateTime(2023, 04, 20, 12, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_skillet_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Skillet",
                PhaseCode = "Phase 01",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 4,
                StartDate = new DateTime(2023, 04, 21, 08, 00, 00),
                EndDate = new DateTime(2023, 04, 21, 12, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 4,
                StartDate = new DateTime(2023, 04, 20, 12, 00, 00),
                EndDate = new DateTime(2023, 04, 20, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 4,
                StartDate = new DateTime(2023, 04, 21, 12, 00, 00),
                EndDate = new DateTime(2023, 04, 21, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 10,
                StartDate = new DateTime(2023, 04, 24, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 24, 18, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 10,
                StartDate = new DateTime(2023, 04, 25, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 25, 18, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 10,
                StartDate = new DateTime(2023, 04, 26, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 26, 18, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 10,
                StartDate = new DateTime(2023, 04, 27, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 27, 18, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "tho.vu1",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 1,
                RateName = "VN Dev",
                RateAmount = 100,
                Hours = 10,
                StartDate = new DateTime(2023, 04, 28, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 28, 18, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "nguyet.trinh",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 2,
                RateName = "VN Test",
                RateAmount = 90,
                Hours = 8,
                StartDate = new DateTime(2023, 04, 24, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 24, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "nguyet.trinh",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 2,
                RateName = "VN Test",
                RateAmount = 90,
                Hours = 8,
                StartDate = new DateTime(2023, 04, 25, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 25, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "nguyet.trinh",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 2,
                RateName = "VN Test",
                RateAmount = 90,
                Hours = 8,
                StartDate = new DateTime(2023, 04, 26, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 26, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "nguyet.trinh",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 2,
                RateName = "VN Test",
                RateAmount = 90,
                Hours = 8,
                StartDate = new DateTime(2023, 04, 27, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 27, 16, 00, 00),
            });
            result.Add(new EmployeeTimesheetEntry()
            {
                EmployeeTimesheetEntryId = employeeTimesheetEntryId++,
                ExternalProjectId = _external_medtech_id,
                ExternalTimesheetId = externalTimesheetId++,
                TimesheetUsername = "nguyet.trinh",
                ProjectName = "Medtech",
                PhaseCode = "Maintain",
                ExternalRateId = 2,
                RateName = "VN Test",
                RateAmount = 90,
                Hours = 8,
                StartDate = new DateTime(2023, 04, 28, 8, 00, 00),
                EndDate = new DateTime(2023, 04, 28, 16, 00, 00),
            });
            return result;
        }

        private List<UserIdLookup> CreateUserIdLookup()
        {
            var id = 1;
            var result = new List<UserIdLookup>();
            result.Add(new UserIdLookup()
            {
                Id = id++,
                BambooHREmail = "tho.vu@codehq.nz",
                BambooHRFirstName = "tho",
                BambooHRLastName = "vu",
                TimesheetUserName = "tho.vu1",
                TimesheetEmail = "tho.vu1@augensoftwaregroup.com"
            });
            result.Add(new UserIdLookup()
            {
                Id = id++,
                BambooHREmail = "nguyet.trinh@codehq.nz",
                BambooHRFirstName = "nguyet",
                BambooHRLastName = "trinh",
                TimesheetUserName = "nguyet.trinh",
                TimesheetEmail = "nguyet.trinh@augensoftwaregroup.com"
            });
            return result;
        }

        private List<PhaseResourceView> CreatePhaseResourceUtilisation()
        {
            var phaseResourceUtilisationId = 1;
            var result = new List<PhaseResourceView>();
            result.Add(new PhaseResourceView()
            {
                PhaseResourceUtilisationId = phaseResourceUtilisationId++,
                Username = "tho.vu",
                Country = "Viet Nam",
                EmployeeId = 1,
                ExternalProjectId = _external_skillet_id,
                ProjectId = 101,
                ProjectName = "Skillet",
                PhaseId = 1,
                PhaseName = "Phase 01",
                PhaseResourceId = 1,
                PhaseResourceUtilisations = new List<PhaseResourceUtilisation>()
                {
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 04, 17),
                        TotalHours = 40
                    },
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 04, 24),
                        TotalHours = 40
                    },
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 05, 01),
                        TotalHours = 16
                    },
                    new PhaseResourceUtilisation()
                    {
                       StartWeek = new DateTime(2023, 05, 08),
                        TotalHours = 40
                    },
                     new PhaseResourceUtilisation()
                    {
                       StartWeek = new DateTime(2023, 05, 15),
                        TotalHours = 40
                    },
                      new PhaseResourceUtilisation()
                    {
                       StartWeek = new DateTime(2023, 05, 22),
                        TotalHours = 24
                    },
                }
            });


            result.Add(new PhaseResourceView()
            {
                PhaseResourceUtilisationId = phaseResourceUtilisationId++,
                Username = "tho.vu",
                Country = "Viet Nam",
                EmployeeId = 1,
                ExternalProjectId = _external_medtech_id,
                ProjectId = 102,
                ProjectName = "Medtech",
                PhaseId = 2,
                PhaseName = "Maintain",
                PhaseResourceId = 1,
                PhaseResourceUtilisations = new List<PhaseResourceUtilisation>()
                {
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 04, 24),
                        TotalHours = 40
                    },
                     new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 05, 1),
                        TotalHours = 16
                    },
                     new PhaseResourceUtilisation()
                    {
                         StartWeek = new DateTime(2023, 05, 8),
                        TotalHours = 40
                    },
                      new PhaseResourceUtilisation()
                    {
                         StartWeek = new DateTime(2023, 05, 15),
                        TotalHours = 40
                    },
                }

            });

            result.Add(new PhaseResourceView()
            {
                PhaseResourceUtilisationId = phaseResourceUtilisationId++,
                Username = "nguyet.trinh",
                Country = "Viet Nam",
                EmployeeId = 2,
                ExternalProjectId = _external_medtech_id,
                ProjectId = 102,
                ProjectName = "Medtech",
                PhaseId = 2,
                PhaseName = "Maintain",
                PhaseResourceId = 1,
                PhaseResourceUtilisations = new List<PhaseResourceUtilisation>()
                {
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 05, 1),
                        TotalHours = 8
                    },
                    new PhaseResourceUtilisation()
                    {
                         StartWeek = new DateTime(2023, 05, 8),
                        TotalHours = 40
                    },
                    new PhaseResourceUtilisation()
                    {
                         StartWeek = new DateTime(2023, 05, 15),
                        TotalHours = 40
                    },
                    new PhaseResourceUtilisation()
                    {
                         StartWeek = new DateTime(2023, 05, 22),
                        TotalHours = 40
                    },
                }
            });

            result.Add(new PhaseResourceView()
            {
                PhaseResourceUtilisationId = phaseResourceUtilisationId++,
                Country = "Viet Nam",
                ExternalProjectId = 333,
                ProjectId = 103,
                ProjectName = "New Project",
                PhaseId = 3,
                PhaseName = "01",
                PhaseResourceId = 1,
                ResourcePlaceHolderId = 1,
                ResourcePlaceHolderName = "VN Developer (Placeholder)",
                PhaseResourceUtilisations = new List<PhaseResourceUtilisation>()
                {
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 05, 22),
                        TotalHours = 40
                    },
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 05, 29),
                        TotalHours = 40
                    },
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 06, 5),
                        TotalHours = 40
                    },
                    new PhaseResourceUtilisation()
                    {
                        StartWeek = new DateTime(2023, 06, 12),
                        TotalHours = 40
                    },
                }

            });
            return result;
        }

        private List<Employee> CreateEmployees()
        {
            var result = new List<Employee>();
            var employeeId = 1;
            result.Add(new Employee()
            {
                UserName = "tho.vu",
                Country = "Viet Nam",
                EmployeeId = employeeId++,
                WorkingHours = "40",
            });
            result.Add(new Employee()
            {
                UserName = "nguyet.trinh",
                Country = "Viet Nam",
                EmployeeId = employeeId++,
                WorkingHours = "40",
            });
            result.Add(new Employee()
            {
                UserName = "nghia.nguyen",
                Country = "Viet Nam",
                EmployeeId = employeeId++,
                WorkingHours = "40",
            });
            return result;
        }
        [Fact]
        public void TC1_Calculate_Actual_Hours_For_Employee_Project()
        {
            var timesheetEntries = CreateActualTimesheetEntries();
            var userIdLookup = CreateUserIdLookup();
            var startDate = new DateTime(2023, 04, 17);
            var resourceUtilisation = _resourceUtilisationService.GetActualResourceUtilisations(timesheetEntries, startDate, userIdLookup);
            resourceUtilisation.Should().NotBeNull();
            resourceUtilisation.Count.Should().Be(3);

            var thovuSkillet = resourceUtilisation[0];
            AssertThoVuAssignedSkillet(thovuSkillet, true, false, false);

            var thovuMedtech = resourceUtilisation[1];
            AssertThoVuAssignedMedtech(thovuMedtech, true, false, false);

            var nguyettrinhMedtech = resourceUtilisation[2];
            AssertNguyetTrinhAssignedMedtech(nguyettrinhMedtech, true, false, false);
        }

        [Fact]
        public void TC1_Calcualte_Phase_Resource_Utilisation()
        {
            var startDate = new DateTime(2023, 04, 17);
            var limitEndDate = startDate.AddDays(7 * 52);
            var phaseResourceUtilisations = CreatePhaseResourceUtilisation();
            var resourceUtilisation = _resourceUtilisationService.GetProjectResourceUtilisations(phaseResourceUtilisations, startDate, limitEndDate);

            resourceUtilisation.Should().NotBeNull();
            resourceUtilisation.Count.Should().Be(4);

            var thovuSkillet = resourceUtilisation[0];
            AssertThoVuAssignedSkillet(thovuSkillet, false, true, false);

            var thovuMedtech = resourceUtilisation[1];
            AssertThoVuAssignedMedtech(thovuMedtech, false, true, false);

            var nguyettrinhMedtech = resourceUtilisation[2];
            AssertNguyetTrinhAssignedMedtech(nguyettrinhMedtech, false, true, false);

            var placeHolderNewProject = resourceUtilisation[3];
            AssertPlaceHolderAssignedNewProject(placeHolderNewProject);
        }

        private static void AssertThoVuAssignedMedtech(ProjectResourceUtilisationModel thovuMedtech,
            bool assertActualOnly, bool assertPhaseUtilisationOnly, bool assertAll)
        {
            var noOfWeek = 0;
            if (assertActualOnly) noOfWeek = 2;
            else if (assertPhaseUtilisationOnly) noOfWeek = 4;
            else if (assertAll) noOfWeek = 5;

            thovuMedtech.UserName.Should().Be("tho.vu");
            thovuMedtech.ProjectName.Should().BeSameAs("Medtech");
            thovuMedtech.UtilisationByWeeks.Count().Should().Be(noOfWeek);

            if (assertActualOnly || assertAll)
            {

                var thovuMedtechWeek1 = thovuMedtech.UtilisationByWeeks[0];
                thovuMedtechWeek1.StartDate.Should().Be(new DateTime(2023, 04, 17));
                thovuMedtechWeek1.Hours.Should().Be(8);
                thovuMedtechWeek1.IsActual.Should().BeTrue();

                var thovuMedtechWeek2 = thovuMedtech.UtilisationByWeeks[1];
                thovuMedtechWeek2.StartDate.Should().Be(new DateTime(2023, 04, 24));
                thovuMedtechWeek2.Hours.Should().Be(50);
                thovuMedtechWeek2.IsActual.Should().BeTrue();
            }

            var idx = assertAll ? 2 : 0;
            if (assertPhaseUtilisationOnly || assertAll)
            {
                if (assertPhaseUtilisationOnly)
                {
                    var thovuMedtechWeek1 = thovuMedtech.UtilisationByWeeks[idx++];
                    thovuMedtechWeek1.StartDate.Should().Be(new DateTime(2023, 04, 24));
                    thovuMedtechWeek1.Hours.Should().Be(40);
                }

                var thovuMedtechWeek2 = thovuMedtech.UtilisationByWeeks[idx++];
                thovuMedtechWeek2.StartDate.Should().Be(new DateTime(2023, 5, 01));
                thovuMedtechWeek2.Hours.Should().Be(16);
                var thovuMedtechWeek3 = thovuMedtech.UtilisationByWeeks[idx++];
                thovuMedtechWeek3.StartDate.Should().Be(new DateTime(2023, 5, 8));
                thovuMedtechWeek3.Hours.Should().Be(40);
                var thovuMedtechWeek4 = thovuMedtech.UtilisationByWeeks[idx++];
                thovuMedtechWeek4.StartDate.Should().Be(new DateTime(2023, 5, 15));
                thovuMedtechWeek4.Hours.Should().Be(40);
            }
        }

        private static void AssertThoVuAssignedSkillet(ProjectResourceUtilisationModel thovuSkillet,
            bool assertActualOnly, bool assertPhaseUtilisationOnly, bool assertAll)
        {
            var noOfWeek = 0;
            if (assertActualOnly) noOfWeek = 1;
            else if (assertPhaseUtilisationOnly) noOfWeek = 6;
            else if (assertAll) noOfWeek = 6;

            thovuSkillet.UserName.Should().Be("tho.vu");
            thovuSkillet.ProjectName.Should().BeSameAs("Skillet");
            thovuSkillet.UtilisationByWeeks.Count().Should().Be(noOfWeek);

            if (assertActualOnly || assertAll)
            {

                var thovuSkilletWeek1 = thovuSkillet.UtilisationByWeeks[0];
                thovuSkilletWeek1.StartDate.Should().Be(new DateTime(2023, 04, 17));
                thovuSkilletWeek1.Hours.Should().Be(32);
                thovuSkilletWeek1.IsActual.Should().BeTrue();
            }

            var idx = assertAll ? 1 : 0;

            if (assertPhaseUtilisationOnly || assertAll)
            {
                if (assertPhaseUtilisationOnly)
                {
                    var thovuSkilletWeek1 = thovuSkillet.UtilisationByWeeks[idx++];
                    thovuSkilletWeek1.StartDate.Should().Be(new DateTime(2023, 04, 17));
                    thovuSkilletWeek1.Hours.Should().Be(40);
                }
                var thovuSkilletWeek2 = thovuSkillet.UtilisationByWeeks[idx++];
                thovuSkilletWeek2.StartDate.Should().Be(new DateTime(2023, 04, 24));
                thovuSkilletWeek2.Hours.Should().Be(40);
                var thovuSkilletWeek3 = thovuSkillet.UtilisationByWeeks[idx++];
                thovuSkilletWeek3.StartDate.Should().Be(new DateTime(2023, 5, 1));
                thovuSkilletWeek3.Hours.Should().Be(16);
                var thovuSkilletWeek4 = thovuSkillet.UtilisationByWeeks[idx++];
                thovuSkilletWeek4.StartDate.Should().Be(new DateTime(2023, 5, 8));
                thovuSkilletWeek4.Hours.Should().Be(40);
                var thovuSkilletWeek5 = thovuSkillet.UtilisationByWeeks[idx++];
                thovuSkilletWeek5.StartDate.Should().Be(new DateTime(2023, 5, 15));
                thovuSkilletWeek5.Hours.Should().Be(40);
                var thovuSkilletWeek6 = thovuSkillet.UtilisationByWeeks[idx++];
                thovuSkilletWeek6.StartDate.Should().Be(new DateTime(2023, 5, 22));
                thovuSkilletWeek6.Hours.Should().Be(24);
            }
        }

        private void AssertNguyetTrinhAssignedMedtech(ProjectResourceUtilisationModel nguyettrinhMedtech,
            bool assertActualOnly, bool assertPhaseUtilisationOnly, bool assertAll)
        {
            var noOfWeek = 0;
            if (assertActualOnly) noOfWeek = 1;
            else if (assertPhaseUtilisationOnly) noOfWeek = 4;
            else if (assertAll) noOfWeek = 5;

            nguyettrinhMedtech.UserName.Should().Be("nguyet.trinh");
            nguyettrinhMedtech.ProjectName.Should().BeSameAs("Medtech");
            nguyettrinhMedtech.UtilisationByWeeks.Count().Should().Be(noOfWeek);

            if (assertActualOnly || assertAll)
            {
                var nguyettrinhMedtechWeek1 = nguyettrinhMedtech.UtilisationByWeeks[0];
                nguyettrinhMedtechWeek1.StartDate.Should().Be(new DateTime(2023, 04, 24));
                nguyettrinhMedtechWeek1.Hours.Should().Be(40);
                nguyettrinhMedtechWeek1.IsActual.Should().BeTrue();
            }
            var idx = assertAll ? 1 : 0;
            if (assertPhaseUtilisationOnly || assertAll)
            {
                var nguyettrinhMedtechWeek1 = nguyettrinhMedtech.UtilisationByWeeks[idx++];
                nguyettrinhMedtechWeek1.StartDate.Should().Be(new DateTime(2023, 5, 01));
                nguyettrinhMedtechWeek1.Hours.Should().Be(8);
                var nguyettrinhMedtechWeek2 = nguyettrinhMedtech.UtilisationByWeeks[idx++];
                nguyettrinhMedtechWeek2.StartDate.Should().Be(new DateTime(2023, 5, 8));
                nguyettrinhMedtechWeek2.Hours.Should().Be(40);
                var nguyettrinhMedtechWeek3 = nguyettrinhMedtech.UtilisationByWeeks[idx++];
                nguyettrinhMedtechWeek3.StartDate.Should().Be(new DateTime(2023, 5, 15));
                nguyettrinhMedtechWeek3.Hours.Should().Be(40);
                var nguyettrinhMedtechWeek4 = nguyettrinhMedtech.UtilisationByWeeks[idx++];
                nguyettrinhMedtechWeek4.StartDate.Should().Be(new DateTime(2023, 5, 22));
                nguyettrinhMedtechWeek4.Hours.Should().Be(40);
            }
        }

        private void AssertPlaceHolderAssignedNewProject(ProjectResourceUtilisationModel placeHolderNewProject)
        {
            placeHolderNewProject.UserName.Should().BeSameAs("VN Developer (Placeholder)");
            placeHolderNewProject.ProjectName.Should().BeSameAs("New Project");
            placeHolderNewProject.UtilisationByWeeks.Count().Should().Be(4);

            var placeHolderNewProjectWeek1 = placeHolderNewProject.UtilisationByWeeks[0];
            placeHolderNewProjectWeek1.StartDate.Should().Be(new DateTime(2023, 5, 22));
            placeHolderNewProjectWeek1.Hours.Should().Be(40);
            var placeHolderNewProjectWeek2 = placeHolderNewProject.UtilisationByWeeks[1];
            placeHolderNewProjectWeek2.StartDate.Should().Be(new DateTime(2023, 5, 29));
            placeHolderNewProjectWeek2.Hours.Should().Be(40);
            var placeHolderNewProjectWeek3 = placeHolderNewProject.UtilisationByWeeks[2];
            placeHolderNewProjectWeek3.StartDate.Should().Be(new DateTime(2023, 6, 5));
            placeHolderNewProjectWeek3.Hours.Should().Be(40);
            var placeHolderNewProjectWeek4 = placeHolderNewProject.UtilisationByWeeks[3];
            placeHolderNewProjectWeek4.StartDate.Should().Be(new DateTime(2023, 6, 12));
            placeHolderNewProjectWeek4.Hours.Should().Be(40);
        }

        //[Fact]
        //public async Task TC1_Get_Resource_Utilisation()
        //{
        //    var startDate = new DateTime(2023, 04, 17);
        //    var (resources, consumedTime) = await _resourceUtilisationService.GetResourceUtilisations(startDate);
        //    resources.Count().Should().Be(4);

        //    var thovu = resources[0];
        //    thovu.UserName.Should().Be("tho.vu");
        //    thovu.ProjectResourceUtilisations.Count().Should().Be(2);
        //    AssertThoVuAssignedSkillet(thovu.ProjectResourceUtilisations[0], false, false, true);
        //    AssertThoVuAssignedMedtech(thovu.ProjectResourceUtilisations[1], false, false, true);

        //    var nguyettrinh = resources[1];
        //    nguyettrinh.UserName.Should().Be("nguyet.trinh");
        //    nguyettrinh.ProjectResourceUtilisations.Count().Should().Be(1);
        //    AssertNguyetTrinhAssignedMedtech(nguyettrinh.ProjectResourceUtilisations[0], false, false, true);

        //    var nghianguyen = resources[2];
        //    nghianguyen.UserName.Should().Be("nghia.nguyen");
        //    nghianguyen.ProjectResourceUtilisations.Count().Should().Be(0);

        //    var placeholder = resources[3];
        //    placeholder.UserName.Should().Be("VN Developer (Placeholder)");
        //    placeholder.ProjectResourceUtilisations.Count().Should().Be(1);
        //    AssertPlaceHolderAssignedNewProject(placeholder.ProjectResourceUtilisations[0]);
        //}
    }
}