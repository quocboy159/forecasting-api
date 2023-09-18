using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class FixEmployeeJobTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobTile",
                table: "Employee",
                newName: "JobTitle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "Employee",
                newName: "JobTile");
        }
    }
}
