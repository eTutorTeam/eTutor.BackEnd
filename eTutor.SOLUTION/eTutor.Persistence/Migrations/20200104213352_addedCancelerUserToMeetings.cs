using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class addedCancelerUserToMeetings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

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
            
        }
    }
}
