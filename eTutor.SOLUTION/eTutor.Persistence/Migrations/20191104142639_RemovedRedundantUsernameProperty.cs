using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class RemovedRedundantUsernameProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2ed231d9-f652-409d-ad77-834a82b4cb5e");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "54180e41-1346-4e8f-aa67-33a1f0021be1");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "117c5ebc-8716-4cbb-b85e-9f70ab947edd");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "8985b6a9-f400-442f-ad5b-315090175527");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9ac69256-3e24-4944-91d8-dfabd887c5fb");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0acb6929-bb44-4365-ba93-a39fd6931157");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "430f281c-280d-456c-8f61-3105f59e18cf");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "c7d22753-4307-4b5b-9947-e3de5f9e5224");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }
    }
}
