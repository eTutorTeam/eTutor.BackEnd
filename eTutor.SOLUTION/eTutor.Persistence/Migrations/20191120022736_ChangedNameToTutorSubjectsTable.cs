using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class ChangedNameToTutorSubjectsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TutorTopics_Subjects_SubjectId",
                table: "TutorTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorTopics_Users_TutorId",
                table: "TutorTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TutorTopics",
                table: "TutorTopics");

            migrationBuilder.RenameTable(
                name: "TutorTopics",
                newName: "TutorSubjects");

            migrationBuilder.RenameIndex(
                name: "IX_TutorTopics_TutorId",
                table: "TutorSubjects",
                newName: "IX_TutorSubjects_TutorId");

            migrationBuilder.RenameIndex(
                name: "IX_TutorTopics_SubjectId",
                table: "TutorSubjects",
                newName: "IX_TutorSubjects_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TutorSubjects",
                table: "TutorSubjects",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "723b9def-4215-46d1-b1b0-36f62acf7b97");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "6da36115-9f00-401a-882c-10d5870ddac1");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "88be5a75-8dd2-4d59-9d80-17a4926f9f50");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "7d97919f-b913-42ea-b452-ddf69057aa7e");

            migrationBuilder.AddForeignKey(
                name: "FK_TutorSubjects_Subjects_SubjectId",
                table: "TutorSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorSubjects_Users_TutorId",
                table: "TutorSubjects",
                column: "TutorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TutorSubjects_Subjects_SubjectId",
                table: "TutorSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorSubjects_Users_TutorId",
                table: "TutorSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TutorSubjects",
                table: "TutorSubjects");

            migrationBuilder.RenameTable(
                name: "TutorSubjects",
                newName: "TutorTopics");

            migrationBuilder.RenameIndex(
                name: "IX_TutorSubjects_TutorId",
                table: "TutorTopics",
                newName: "IX_TutorTopics_TutorId");

            migrationBuilder.RenameIndex(
                name: "IX_TutorSubjects_SubjectId",
                table: "TutorTopics",
                newName: "IX_TutorTopics_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TutorTopics",
                table: "TutorTopics",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1fcb3117-61cd-47ce-a27f-94cf3b22ec14");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3a95437f-d55d-4f98-91e2-63c7e65b0073");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "60ec79fd-0c92-4f0e-a71d-a6ebf93f37ca");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "6334dd34-87f2-4f48-8a87-946cffa2d6f4");

            migrationBuilder.AddForeignKey(
                name: "FK_TutorTopics_Subjects_SubjectId",
                table: "TutorTopics",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorTopics_Users_TutorId",
                table: "TutorTopics",
                column: "TutorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
