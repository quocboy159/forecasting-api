using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class RemoveProjectManagerIdInEmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Employee",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectManagerId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectManagerId",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectManagerId",
                table: "Project",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Employee",
                table: "Project",
                column: "ProjectManagerId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");
        }
    }
}
