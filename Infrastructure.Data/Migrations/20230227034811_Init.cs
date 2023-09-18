using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    ClientType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    DateJoined = table.Column<DateTime>(type: "date", nullable: true),
                    DateLeave = table.Column<DateTime>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ActiveStatus = table.Column<bool>(type: "bit", nullable: true),
                    BranchLocation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Department = table.Column<int>(type: "int", nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "PhaseResourceUtilisation",
                columns: table => new
                {
                    PhaseResourceUtilisationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekStartDate = table.Column<DateTime>(type: "date", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseResourceUtilisation", x => x.PhaseResourceUtilisationId);
                });

            migrationBuilder.CreateTable(
                name: "PublicHoliday",
                columns: table => new
                {
                    PublidHolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    Country = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHoliday", x => x.PublidHolidayId);
                });

            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    RateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.RateId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DefaultRate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SkillsetCategory",
                columns: table => new
                {
                    SkillsetCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsetCategory", x => x.SkillsetCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ProjectCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ProjectType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    ProjectValue = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    Confident = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ProjectManagerId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Project_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Project_Employee",
                        column: x => x.ProjectManagerId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    SalaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salary", x => x.SalaryId);
                    table.ForeignKey(
                        name: "FK_Salary_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRole",
                columns: table => new
                {
                    EmployeeRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRole", x => x.EmployeeRoleId);
                    table.ForeignKey(
                        name: "FK_EmployeeRole_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_EmployeeRole_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Skillset",
                columns: table => new
                {
                    SkillsetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillsetCategoryId = table.Column<int>(type: "int", nullable: false),
                    SkillsetName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    SkillsetTypeId = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skillset", x => x.SkillsetId);
                    table.ForeignKey(
                        name: "FK_Skillset_SkillsetCategory",
                        column: x => x.SkillsetCategoryId,
                        principalTable: "SkillsetCategory",
                        principalColumn: "SkillsetCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Phase",
                columns: table => new
                {
                    PhaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhaseName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimesheetPhaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phase", x => x.PhaseId);
                    table.ForeignKey(
                        name: "FK_Phase_Project",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "ProjectRate",
                columns: table => new
                {
                    ProjectRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRate", x => x.ProjectRateId);
                    table.ForeignKey(
                        name: "FK_ProjectRate_Project",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkillset",
                columns: table => new
                {
                    EmployeeSkillsetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillsetId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    ProficiencyLevel = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ActiveStatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkillset", x => x.EmployeeSkillsetId);
                    table.ForeignKey(
                        name: "FK_EmployeeSkillset_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_EmployeeSkillset_Skillset",
                        column: x => x.SkillsetId,
                        principalTable: "Skillset",
                        principalColumn: "SkillsetId");
                });

            migrationBuilder.CreateTable(
                name: "PhaseSkillset",
                columns: table => new
                {
                    PhaseSkillSetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhaseId = table.Column<int>(type: "int", nullable: false),
                    SkillsetId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSkillset", x => x.PhaseSkillSetId);
                    table.ForeignKey(
                        name: "FK_PhaseSkillset_Phase",
                        column: x => x.PhaseId,
                        principalTable: "Phase",
                        principalColumn: "PhaseId");
                    table.ForeignKey(
                        name: "FK_PhaseSkillset_Skillset",
                        column: x => x.SkillsetId,
                        principalTable: "Skillset",
                        principalColumn: "SkillsetId");
                });

            migrationBuilder.CreateTable(
                name: "PhaseResource",
                columns: table => new
                {
                    PhaseResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PhaseId = table.Column<int>(type: "int", nullable: false),
                    ProjectRateId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    UtilisationId = table.Column<int>(type: "int", nullable: true),
                    ResourceStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModiffiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseResource", x => x.PhaseResourceId);
                    table.ForeignKey(
                        name: "FK_PhaseResource_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_PhaseResource_Phase",
                        column: x => x.PhaseId,
                        principalTable: "Phase",
                        principalColumn: "PhaseId");
                    table.ForeignKey(
                        name: "FK_PhaseResource_PhaseResourceUtilisation",
                        column: x => x.UtilisationId,
                        principalTable: "PhaseResourceUtilisation",
                        principalColumn: "PhaseResourceUtilisationId");
                    table.ForeignKey(
                        name: "FK_PhaseResource_ProjectRate",
                        column: x => x.ProjectRateId,
                        principalTable: "ProjectRate",
                        principalColumn: "ProjectRateId");
                });

            migrationBuilder.CreateTable(
                name: "ProjectRateHistory",
                columns: table => new
                {
                    ProjectRateHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateId = table.Column<int>(type: "int", nullable: false),
                    ExternalRateHistoryId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRateHistory", x => x.ProjectRateHistoryId);
                    table.ForeignKey(
                        name: "FK_ProjectRateHistory_ProjectRate",
                        column: x => x.RateId,
                        principalTable: "ProjectRate",
                        principalColumn: "ProjectRateId");
                });

            migrationBuilder.CreateTable(
                name: "PhaseResourceException",
                columns: table => new
                {
                    PhaseResourceExceptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhaseResourceId = table.Column<int>(type: "int", nullable: false),
                    StartWeek = table.Column<DateTime>(type: "date", nullable: false),
                    EndWeek = table.Column<DateTime>(type: "date", nullable: false),
                    HoursPerWeek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseResourceExceptionId", x => x.PhaseResourceExceptionId);
                    table.ForeignKey(
                        name: "FK_PhaseResourceException_PhaseResource_PhaseResourceId",
                        column: x => x.PhaseResourceId,
                        principalTable: "PhaseResource",
                        principalColumn: "PhaseResourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_ClientName",
                table: "Client",
                column: "ClientName",
                unique: true,
                filter: "[ClientName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_EmployeeId",
                table: "EmployeeRole",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_RoleId",
                table: "EmployeeRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillset_EmployeeId",
                table: "EmployeeSkillset",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillset_SkillsetId",
                table: "EmployeeSkillset",
                column: "SkillsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Phase_ProjectId",
                table: "Phase",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseResource_EmployeeId",
                table: "PhaseResource",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseResource_PhaseId",
                table: "PhaseResource",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseResource_ProjectRateId",
                table: "PhaseResource",
                column: "ProjectRateId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseResource_UtilisationId",
                table: "PhaseResource",
                column: "UtilisationId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseResourceException_PhaseResourceId",
                table: "PhaseResourceException",
                column: "PhaseResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseSkillset_PhaseId",
                table: "PhaseSkillset",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseSkillset_SkillsetId",
                table: "PhaseSkillset",
                column: "SkillsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ClientId",
                table: "Project",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectManagerId",
                table: "Project",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectName",
                table: "Project",
                column: "ProjectName",
                unique: true,
                filter: "[ProjectName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRate_ProjectId",
                table: "ProjectRate",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRateHistory_RateId",
                table: "ProjectRateHistory",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_EmployeeId",
                table: "Salary",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Skillset_SkillsetCategoryId",
                table: "Skillset",
                column: "SkillsetCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeRole");

            migrationBuilder.DropTable(
                name: "EmployeeSkillset");

            migrationBuilder.DropTable(
                name: "PhaseResourceException");

            migrationBuilder.DropTable(
                name: "PhaseSkillset");

            migrationBuilder.DropTable(
                name: "ProjectRateHistory");

            migrationBuilder.DropTable(
                name: "PublicHoliday");

            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "PhaseResource");

            migrationBuilder.DropTable(
                name: "Skillset");

            migrationBuilder.DropTable(
                name: "Phase");

            migrationBuilder.DropTable(
                name: "PhaseResourceUtilisation");

            migrationBuilder.DropTable(
                name: "ProjectRate");

            migrationBuilder.DropTable(
                name: "SkillsetCategory");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
