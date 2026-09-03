using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FactoryFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialMachines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "Code", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "MC-001", "CNC", "Idle" },
                    { 2, "MC-002", "Laser", "Idle" },
                    { 3, "MC-003", "Packing", "Idle" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
