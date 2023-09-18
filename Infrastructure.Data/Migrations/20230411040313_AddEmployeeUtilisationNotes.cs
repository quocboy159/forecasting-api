using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class AddEmployeeUtilisationNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeUtilisationNotes",
                columns: table => new
                {
                    EmployeeUtilisationNotesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ForecastWarning = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    InternalWorkNotes = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    OtherNotes = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeUtilisationNotes", x => x.EmployeeUtilisationNotesId);
                    table.ForeignKey(
                        name: "FK_EmployeeUtilisationNotes_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeUtilisationNotes_EmployeeId",
                table: "EmployeeUtilisationNotes",
                column: "EmployeeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeUtilisationNotes");
        }
    }
}
