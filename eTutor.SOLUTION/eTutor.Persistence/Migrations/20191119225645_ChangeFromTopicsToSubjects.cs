using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class ChangeFromTopicsToSubjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Topics_TopicId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicInterests_Topics_TopicId",
                table: "TopicInterests");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorTopics_Topics_TopicId",
                table: "TutorTopics");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_TutorTopics_TopicId",
                table: "TutorTopics");

            migrationBuilder.DropIndex(
                name: "IX_TopicInterests_TopicId",
                table: "TopicInterests");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_TopicId",
                table: "Meetings");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "TutorTopics",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "TopicInterests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Meetings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e5cf09a6-cb20-422b-b508-ad44148b0576");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "5ee8f3ef-1eec-4964-aefc-0ef32d191568");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "c77cf769-3cfb-4f04-bdf4-9ecb1329b51d");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "828a538c-95f7-4617-9ff8-ea737054eca7");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTopics_SubjectId",
                table: "TutorTopics",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicInterests_SubjectId",
                table: "TopicInterests",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_SubjectId",
                table: "Meetings",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Subjects_SubjectId",
                table: "Meetings",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicInterests_Subjects_SubjectId",
                table: "TopicInterests",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorTopics_Subjects_SubjectId",
                table: "TutorTopics",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Subjects_SubjectId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicInterests_Subjects_SubjectId",
                table: "TopicInterests");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorTopics_Subjects_SubjectId",
                table: "TutorTopics");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_TutorTopics_SubjectId",
                table: "TutorTopics");

            migrationBuilder.DropIndex(
                name: "IX_TopicInterests_SubjectId",
                table: "TopicInterests");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_SubjectId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "TutorTopics");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "TopicInterests");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Meetings");

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "dac724af-9ff7-4649-a3d2-da5f22df17b8");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "4940e081-5158-4365-8fc3-3aa904733160");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "aaf0b375-6732-46ce-b9d4-0849e9b025fc");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "bcd2914b-45a5-46c9-8ee1-621679e68608");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTopics_TopicId",
                table: "TutorTopics",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicInterests_TopicId",
                table: "TopicInterests",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_TopicId",
                table: "Meetings",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Topics_TopicId",
                table: "Meetings",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicInterests_Topics_TopicId",
                table: "TopicInterests",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorTopics_Topics_TopicId",
                table: "TutorTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
