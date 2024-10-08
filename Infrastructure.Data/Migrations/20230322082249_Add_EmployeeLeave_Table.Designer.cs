﻿// <auto-generated />
using System;
using ForecastingSystem.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ForecastingSystemDbContext))]
    [Migration("20230322082249_Add_EmployeeLeave_Table")]
    partial class Add_EmployeeLeave_Table
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientId"), 1L, 1);

                    b.Property<string>("ClientName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ClientType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("ExternalClientId")
                        .HasColumnType("int");

                    b.HasKey("ClientId");

                    b.HasIndex("ClientName")
                        .IsUnique()
                        .HasFilter("[ClientName] IS NOT NULL");

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.DataSyncProcess", b =>
                {
                    b.Property<int>("DataSyncProcessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DataSyncProcessId"), 1L, 1);

                    b.Property<string>("DataSyncType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FinishDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastSyncDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Source")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Target")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DataSyncProcessId");

                    b.ToTable("DataSyncProcess", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.DefaultResourcePlaceHolder", b =>
                {
                    b.Property<int>("DefaultResourcePlaceHolderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DefaultResourcePlaceHolderId"), 1L, 1);

                    b.Property<string>("Country")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("DefaultResourcePlaceHolderId");

                    b.ToTable("DefaultResourcePlaceHolder", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"), 1L, 1);

                    b.Property<bool?>("ActiveStatus")
                        .HasColumnType("bit");

                    b.Property<string>("BranchLocation")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateJoined")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DateLeave")
                        .HasColumnType("date");

                    b.Property<int?>("Department")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Dob")
                        .HasColumnType("date")
                        .HasColumnName("DOB");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("ExternalId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Gender")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("LastSyncDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.EmployeeLeave", b =>
                {
                    b.Property<int>("EmployeeLeaveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeLeaveId"), 1L, 1);

                    b.Property<string>("DayType")
                        .HasMaxLength(20)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExternalLeaveId")
                        .HasColumnType("int");

                    b.Property<string>("LeaveCode")
                        .HasMaxLength(10)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasMaxLength(20)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("TotalDays")
                        .HasColumnType("float");

                    b.Property<string>("UserName")
                        .HasMaxLength(255)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("EmployeeLeaveId");

                    b.ToTable("EmployeeLeave", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.EmployeeSkillset", b =>
                {
                    b.Property<int>("EmployeeSkillsetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeSkillsetId"), 1L, 1);

                    b.Property<bool?>("ActiveStatus")
                        .HasColumnType("bit");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("Note")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<int?>("ProficiencyLevel")
                        .HasColumnType("int");

                    b.Property<int>("SkillsetId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("EmployeeSkillsetId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("SkillsetId");

                    b.ToTable("EmployeeSkillset", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Phase", b =>
                {
                    b.Property<int>("PhaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhaseId"), 1L, 1);

                    b.Property<decimal?>("Budget")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("ExternalPhaseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("PhaseName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("TimesheetPhaseId")
                        .HasColumnType("int");

                    b.HasKey("PhaseId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Phase", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseResource", b =>
                {
                    b.Property<int>("PhaseResourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhaseResourceId"), 1L, 1);

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<double>("FTE")
                        .HasColumnType("float");

                    b.Property<int>("HoursPerWeek")
                        .HasColumnType("int");

                    b.Property<string>("LastModiffiedBy")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastSyncDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PhaseId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectRateId")
                        .HasColumnType("int");

                    b.Property<int?>("ResourcePlaceHolderId")
                        .HasColumnType("int");

                    b.Property<string>("ResourceStatus")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("date");

                    b.Property<int?>("UtilisationId")
                        .HasColumnType("int");

                    b.HasKey("PhaseResourceId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PhaseId");

                    b.HasIndex("ProjectRateId");

                    b.HasIndex("UtilisationId");

                    b.ToTable("PhaseResource", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseResourceException", b =>
                {
                    b.Property<int>("PhaseResourceExceptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhaseResourceExceptionId"), 1L, 1);

                    b.Property<int>("HoursPerWeek")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfWeeks")
                        .HasColumnType("int");

                    b.Property<int>("PhaseResourceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartWeek")
                        .HasColumnType("date");

                    b.HasKey("PhaseResourceExceptionId")
                        .HasName("PK_PhaseResourceExceptionId");

                    b.HasIndex("PhaseResourceId");

                    b.ToTable("PhaseResourceException", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseResourceUtilisation", b =>
                {
                    b.Property<int>("PhaseResourceUtilisationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhaseResourceUtilisationId"), 1L, 1);

                    b.Property<int?>("Hours")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<DateTime>("WeekStartDate")
                        .HasColumnType("date");

                    b.HasKey("PhaseResourceUtilisationId");

                    b.ToTable("PhaseResourceUtilisation", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseSkillset", b =>
                {
                    b.Property<int>("PhaseSkillSetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhaseSkillSetId"), 1L, 1);

                    b.Property<int?>("Level")
                        .HasColumnType("int");

                    b.Property<int>("PhaseId")
                        .HasColumnType("int");

                    b.Property<int>("SkillsetId")
                        .HasColumnType("int");

                    b.HasKey("PhaseSkillSetId")
                        .HasName("PK_ProjectSkillset");

                    b.HasIndex("PhaseId");

                    b.HasIndex("SkillsetId");

                    b.ToTable("PhaseSkillset", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"), 1L, 1);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CloseDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Confident")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("ExternalProjectId")
                        .HasColumnType("int");

                    b.Property<string>("ProjectCode")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("ProjectManagerId")
                        .HasColumnType("int");

                    b.Property<string>("ProjectName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProjectType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal?>("ProjectValue")
                        .HasColumnType("decimal(18,0)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ProjectId");

                    b.HasIndex("ClientId");

                    b.HasIndex("ProjectManagerId");

                    b.HasIndex("ProjectName");

                    b.ToTable("Project", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.ProjectRate", b =>
                {
                    b.Property<int>("ProjectRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectRateId"), 1L, 1);

                    b.Property<int?>("ExternalProjectRateId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("RateName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("ProjectRateId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectRate", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.ProjectRateHistory", b =>
                {
                    b.Property<int>("ProjectRateHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectRateHistoryId"), 1L, 1);

                    b.Property<int>("ExternalRateHistoryId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectRateId")
                        .HasColumnType("int");

                    b.Property<double?>("Rate")
                        .HasColumnType("float");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("ProjectRateHistoryId");

                    b.HasIndex("ProjectRateId");

                    b.ToTable("ProjectRateHistory", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PublicHoliday", b =>
                {
                    b.Property<int>("PublicHolidayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PublicHolidayId"), 1L, 1);

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("date");

                    b.Property<DateTime?>("LastSyncDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

                    b.HasKey("PublicHolidayId");

                    b.ToTable("PublicHoliday", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Rate", b =>
                {
                    b.Property<int>("RateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RateId"), 1L, 1);

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("decimal(18,0)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("RateName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("RateId");

                    b.ToTable("Rate", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.ResourcePlaceHolder", b =>
                {
                    b.Property<int>("ResourcePlaceHolderId")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ResourcePlaceHolderName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ResourcePlaceHolderId")
                        .HasName("PK_ResourcePlaceHolderId");

                    b.ToTable("ResourcePlaceHolder", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"), 1L, 1);

                    b.Property<int?>("DefaultRate")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("RoleName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("RoleId");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Salary", b =>
                {
                    b.Property<int>("SalaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SalaryId"), 1L, 1);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<int?>("ExternalId")
                        .HasColumnType("int")
                        .HasColumnName("ExternalID");

                    b.Property<DateTime?>("LastSyncDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Salary1")
                        .HasColumnType("int")
                        .HasColumnName("Salary");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("SalaryId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Salary", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Skillset", b =>
                {
                    b.Property<int>("SkillsetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SkillsetId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<int>("SkillsetCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("SkillsetName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SkillsetTypeId")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength();

                    b.HasKey("SkillsetId");

                    b.HasIndex("SkillsetCategoryId");

                    b.ToTable("Skillset", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.SkillsetCategory", b =>
                {
                    b.Property<int>("SkillsetCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SkillsetCategoryId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Description")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.HasKey("SkillsetCategoryId");

                    b.ToTable("SkillsetCategory", (string)null);
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.EmployeeSkillset", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.Employee", "Employee")
                        .WithMany("EmployeeSkillsets")
                        .HasForeignKey("EmployeeId")
                        .HasConstraintName("FK_EmployeeSkillset_Employee");

                    b.HasOne("ForecastingSystem.Domain.Models.Skillset", "Skillset")
                        .WithMany("EmployeeSkillsets")
                        .HasForeignKey("SkillsetId")
                        .IsRequired()
                        .HasConstraintName("FK_EmployeeSkillset_Skillset");

                    b.Navigation("Employee");

                    b.Navigation("Skillset");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Phase", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.Project", "Project")
                        .WithMany("Phases")
                        .HasForeignKey("ProjectId")
                        .IsRequired()
                        .HasConstraintName("FK_Phase_Project");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseResource", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.Employee", "Employee")
                        .WithMany("PhaseResources")
                        .HasForeignKey("EmployeeId")
                        .HasConstraintName("FK_PhaseResource_Employee");

                    b.HasOne("ForecastingSystem.Domain.Models.Phase", "Phase")
                        .WithMany("PhaseResources")
                        .HasForeignKey("PhaseId")
                        .IsRequired()
                        .HasConstraintName("FK_PhaseResource_Phase");

                    b.HasOne("ForecastingSystem.Domain.Models.ProjectRate", "ProjectRate")
                        .WithMany("PhaseResources")
                        .HasForeignKey("ProjectRateId")
                        .IsRequired()
                        .HasConstraintName("FK_PhaseResource_ProjectRate");

                    b.HasOne("ForecastingSystem.Domain.Models.PhaseResourceUtilisation", "PhaseResourceUtilisation")
                        .WithMany("PhaseResources")
                        .HasForeignKey("UtilisationId")
                        .HasConstraintName("FK_PhaseResource_PhaseResourceUtilisation");

                    b.Navigation("Employee");

                    b.Navigation("Phase");

                    b.Navigation("PhaseResourceUtilisation");

                    b.Navigation("ProjectRate");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseResourceException", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.PhaseResource", "PhaseResource")
                        .WithMany("PhaseResourceExceptions")
                        .HasForeignKey("PhaseResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PhaseResource");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseSkillset", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.Phase", "Phase")
                        .WithMany("PhaseSkillsets")
                        .HasForeignKey("PhaseId")
                        .IsRequired()
                        .HasConstraintName("FK_PhaseSkillset_Phase");

                    b.HasOne("ForecastingSystem.Domain.Models.Skillset", "Skillset")
                        .WithMany("PhaseSkillsets")
                        .HasForeignKey("SkillsetId")
                        .IsRequired()
                        .HasConstraintName("FK_PhaseSkillset_Skillset");

                    b.Navigation("Phase");

                    b.Navigation("Skillset");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Project", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.Client", "Client")
                        .WithMany("Projects")
                        .HasForeignKey("ClientId")
                        .IsRequired()
                        .HasConstraintName("FK_Project_Client");

                    b.HasOne("ForecastingSystem.Domain.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("ProjectManagerId")
                        .HasConstraintName("FK_Project_Employee");

                    b.Navigation("Client");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.ProjectRate", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.Project", "Project")
                        .WithMany("ProjectRates")
                        .HasForeignKey("ProjectId")
                        .IsRequired()
                        .HasConstraintName("FK_ProjectRate_Project");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.ProjectRateHistory", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.ProjectRate", "RateNavigation")
                        .WithMany("ProjectRateHistories")
                        .HasForeignKey("ProjectRateId")
                        .IsRequired()
                        .HasConstraintName("FK_ProjectRateHistory_ProjectRate");

                    b.Navigation("RateNavigation");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.ResourcePlaceHolder", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.PhaseResource", "PhaseResource")
                        .WithOne("ResourcePlaceHolder")
                        .HasForeignKey("ForecastingSystem.Domain.Models.ResourcePlaceHolder", "ResourcePlaceHolderId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("PhaseResource");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Salary", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.Employee", "Employee")
                        .WithMany("Salaries")
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_Salary_Employee");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Skillset", b =>
                {
                    b.HasOne("ForecastingSystem.Domain.Models.SkillsetCategory", "SkillsetCategory")
                        .WithMany("Skillsets")
                        .HasForeignKey("SkillsetCategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_Skillset_SkillsetCategory");

                    b.Navigation("SkillsetCategory");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Client", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Employee", b =>
                {
                    b.Navigation("EmployeeSkillsets");

                    b.Navigation("PhaseResources");

                    b.Navigation("Salaries");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Phase", b =>
                {
                    b.Navigation("PhaseResources");

                    b.Navigation("PhaseSkillsets");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseResource", b =>
                {
                    b.Navigation("PhaseResourceExceptions");

                    b.Navigation("ResourcePlaceHolder");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.PhaseResourceUtilisation", b =>
                {
                    b.Navigation("PhaseResources");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Project", b =>
                {
                    b.Navigation("Phases");

                    b.Navigation("ProjectRates");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.ProjectRate", b =>
                {
                    b.Navigation("PhaseResources");

                    b.Navigation("ProjectRateHistories");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.Skillset", b =>
                {
                    b.Navigation("EmployeeSkillsets");

                    b.Navigation("PhaseSkillsets");
                });

            modelBuilder.Entity("ForecastingSystem.Domain.Models.SkillsetCategory", b =>
                {
                    b.Navigation("Skillsets");
                });
#pragma warning restore 612, 618
        }
    }
}
