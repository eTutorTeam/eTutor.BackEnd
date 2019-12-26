using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class FixToAuthorizationsRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentAuthorizations_Users_ParentId",
                table: "ParentAuthorizations");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "ParentAuthorizations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ParentAuthorizations_Users_ParentId",
                table: "ParentAuthorizations",
                column: "ParentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentAuthorizations_Users_ParentId",
                table: "ParentAuthorizations");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "ParentAuthorizations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
            

            migrationBuilder.AddForeignKey(
                name: "FK_ParentAuthorizations_Users_ParentId",
                table: "ParentAuthorizations",
                column: "ParentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
