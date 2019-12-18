using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class AddManyThingsForMeetingsAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_ParentAutorizations_ParentAutorizationId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Subjects_SubjectId",
                table: "Meetings");

            migrationBuilder.DropTable(
                name: "ParentAutorizations");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Meetings");

            migrationBuilder.RenameColumn(
                name: "ParentAutorizationId",
                table: "Meetings",
                newName: "ParentAuthorizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_ParentAutorizationId",
                table: "Meetings",
                newName: "IX_Meetings_ParentAuthorizationId");

            migrationBuilder.AddColumn<string>(
                name: "AboutMe",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Meetings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ParentAuthorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    AuthorizationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentAuthorizations_Users_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f5f41e8c-978b-4d50-8d26-dba9580b1718");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "df8acf69-a73c-45b1-ae2e-a8cc55e93f40");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "fdfbca9c-dfe2-4540-8d1d-9f08171c800a");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "bc070805-2e30-4c2c-93a7-2771d8f66b36");

            migrationBuilder.CreateIndex(
                name: "IX_ParentAuthorizations_ParentId",
                table: "ParentAuthorizations",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_ParentAuthorizations_ParentAuthorizationId",
                table: "Meetings",
                column: "ParentAuthorizationId",
                principalTable: "ParentAuthorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Subjects_SubjectId",
                table: "Meetings",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_ParentAuthorizations_ParentAuthorizationId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Subjects_SubjectId",
                table: "Meetings");

            migrationBuilder.DropTable(
                name: "ParentAuthorizations");

            migrationBuilder.DropColumn(
                name: "AboutMe",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ParentAuthorizationId",
                table: "Meetings",
                newName: "ParentAutorizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_ParentAuthorizationId",
                table: "Meetings",
                newName: "IX_Meetings_ParentAutorizationId");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Meetings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Meetings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ParentAutorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorizationDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentAutorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentAutorizations_Users_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "972f2f59-11dd-4618-aef2-61007ea62d04");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a18b65ba-318e-4f21-b25e-e3c4ef38ca2c");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9ca09e94-415c-4378-8761-47cf8a5d1a1f");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "2479beb0-0c36-41fd-9811-adf3b82abef6");

            migrationBuilder.CreateIndex(
                name: "IX_ParentAutorizations_ParentId",
                table: "ParentAutorizations",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_ParentAutorizations_ParentAutorizationId",
                table: "Meetings",
                column: "ParentAutorizationId",
                principalTable: "ParentAutorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Subjects_SubjectId",
                table: "Meetings",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
