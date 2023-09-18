using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class Remove_LastSyncDate_In_Client_Project_Phase_ProjectRate_ProjectRateHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                table: "ProjectRateHistory");

            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                table: "ProjectRate");

            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                table: "Phase");

            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "ProjectRate",
                newName: "ExternalProjectRateId");

            migrationBuilder.RenameColumn(
                name: "LastSyncDate",
                table: "Project",
                newName: "CompletionDate");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "Project",
                newName: "ExternalProjectId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CloseDate",
                table: "Project",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalPhaseId",
                table: "Phase",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalClientId",
                table: "Client",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseDate",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ExternalPhaseId",
                table: "Phase");

            migrationBuilder.DropColumn(
                name: "ExternalClientId",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "ExternalProjectRateId",
                table: "ProjectRate",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                name: "ExternalProjectId",
                table: "Project",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                name: "CompletionDate",
                table: "Project",
                newName: "LastSyncDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                table: "ProjectRateHistory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                table: "ProjectRate",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                table: "Phase",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                table: "Client",
                type: "datetime2",
                nullable: true);
        }
    }
}
