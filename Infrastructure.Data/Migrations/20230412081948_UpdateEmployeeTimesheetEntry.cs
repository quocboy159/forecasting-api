using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class UpdateEmployeeTimesheetEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalTimesheetEntryIds",
                table: "EmployeeTimesheetEntry");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "EmployeeTimesheetEntry",
                newName: "TimesheetUsername");

            migrationBuilder.RenameColumn(
                name: "TotalHours",
                table: "EmployeeTimesheetEntry",
                newName: "Hours");

            migrationBuilder.RenameColumn(
                name: "StartWeek",
                table: "EmployeeTimesheetEntry",
                newName: "StartDate");

            migrationBuilder.AlterColumn<int>(
                name: "ExternalRateId",
                table: "EmployeeTimesheetEntry",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "EmployeeTimesheetEntry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ExternalTimesheetId",
                table: "EmployeeTimesheetEntry",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "EmployeeTimesheetEntry");

            migrationBuilder.DropColumn(
                name: "ExternalTimesheetId",
                table: "EmployeeTimesheetEntry");

            migrationBuilder.RenameColumn(
                name: "TimesheetUsername",
                table: "EmployeeTimesheetEntry",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "EmployeeTimesheetEntry",
                newName: "StartWeek");

            migrationBuilder.RenameColumn(
                name: "Hours",
                table: "EmployeeTimesheetEntry",
                newName: "TotalHours");

            migrationBuilder.AlterColumn<int>(
                name: "ExternalRateId",
                table: "EmployeeTimesheetEntry",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalTimesheetEntryIds",
                table: "EmployeeTimesheetEntry",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
