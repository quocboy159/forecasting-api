using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class RemoveSeedRateInConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "RateId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "RateId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "RateId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "RateId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "RateId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "RateId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "RateId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "RateId",
                keyValue: 8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rate",
                columns: new[] { "RateId", "HourlyRate", "IsActive", "RateName", "UpdatedDateTime" },
                values: new object[,]
                {
                    { 1, 195m, true, "NZ Project Manager", new DateTime(2023, 2, 28, 4, 42, 21, 788, DateTimeKind.Utc).AddTicks(870) },
                    { 2, 195m, true, "NZ Architect", new DateTime(2023, 2, 28, 4, 42, 21, 788, DateTimeKind.Utc).AddTicks(877) },
                    { 3, 170m, true, "NZ Tech Lead", new DateTime(2023, 2, 28, 4, 42, 21, 788, DateTimeKind.Utc).AddTicks(879) },
                    { 4, 165m, true, "NZ Developer", new DateTime(2023, 2, 28, 4, 42, 21, 788, DateTimeKind.Utc).AddTicks(881) },
                    { 5, 165m, true, "NZ Business Analyst", new DateTime(2023, 2, 28, 4, 42, 21, 788, DateTimeKind.Utc).AddTicks(883) },
                    { 6, 85m, true, "VN Developer", new DateTime(2023, 2, 28, 4, 42, 21, 788, DateTimeKind.Utc).AddTicks(885) },
                    { 7, 85m, true, "VN Tester", new DateTime(2023, 2, 28, 4, 42, 21, 788, DateTimeKind.Utc).AddTicks(887) },
                    { 8, 90m, true, "VN Tech Lead", new DateTime(2023, 2, 28, 4, 42, 21, 788, DateTimeKind.Utc).AddTicks(889) }
                });
        }
    }
}
