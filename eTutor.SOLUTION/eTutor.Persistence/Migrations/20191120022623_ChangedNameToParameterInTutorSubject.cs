using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class ChangedNameToParameterInTutorSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TutorTopics_Subjects_SubjectId",
                table: "TutorTopics");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "TutorTopics");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "TutorTopics",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TutorTopics_Subjects_SubjectId",
                table: "TutorTopics");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "TutorTopics",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "TutorTopics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6f61c212-b02f-4062-b743-e2af22a186bc");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a5e4c90d-ac79-4fb4-aee4-b98c9ec41cad");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "69a4f3a4-2900-4e98-af39-912c6e4e9966");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "fd5e5489-55ce-49cf-be0d-aa763d5322bd");

            migrationBuilder.AddForeignKey(
                name: "FK_TutorTopics_Subjects_SubjectId",
                table: "TutorTopics",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
