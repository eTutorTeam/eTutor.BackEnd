using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class addedCancelerUserToMeetings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6d487de9-b0b6-4eb6-b249-71bec603ebc1");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "674518cc-a2be-4265-9443-9cb93b1f11d1");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f9b2499a-5352-439a-af1f-4b84f0666014");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "f81396ef-2898-4c6a-accf-7194c240f8b7");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_CancelerUserId",
                table: "Meetings",
                column: "CancelerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Users_CancelerUserId",
                table: "Meetings",
                column: "CancelerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Users_CancelerUserId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_CancelerUserId",
                table: "Meetings");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "adb1cb81-ea82-4873-ac82-a53375477c34");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "612a4c37-3410-4651-a6b8-b0fd9589315d");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "dc771a8d-c1a5-4ebe-bd36-157894c992e8");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "5fbca20d-2109-4c1c-a7ae-0647213f38cd");
        }
    }
}
