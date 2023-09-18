using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class Add_HoursPerWeek_FTE_In_PhaseResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "PhaseResource",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "FTE",
                table: "PhaseResource",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "HoursPerWeek",
                table: "PhaseResource",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourcePlaceHolderId",
                table: "PhaseResource",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResourcePlaceHolder",
                columns: table => new
                {
                    ResourcePlaceHolderId = table.Column<int>(type: "int", nullable: false),
                    ResourcePlaceHolderName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcePlaceHolderId", x => x.ResourcePlaceHolderId);
                    table.ForeignKey(
                        name: "FK_ResourcePlaceHolder_PhaseResource_ResourcePlaceHolderId",
                        column: x => x.ResourcePlaceHolderId,
                        principalTable: "PhaseResource",
                        principalColumn: "PhaseResourceId");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourcePlaceHolder");

            migrationBuilder.DropColumn(
                name: "FTE",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "HoursPerWeek",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "ResourcePlaceHolderId",
                table: "PhaseResource");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "PhaseResource",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
