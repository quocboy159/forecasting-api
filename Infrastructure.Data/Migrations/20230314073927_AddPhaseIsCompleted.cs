using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class AddPhaseIsCompleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Phase",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Phase");
        }
    }
}
