using Microsoft.EntityFrameworkCore.Migrations;

namespace eTutor.Persistence.Migrations
{
    public partial class AddedProfileImageFieldToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "Users",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d3cf4bbd-43c9-44ea-921c-f8b08024613f");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "48f0abaa-989e-4771-83b0-98ae47e29d03");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d1e09f0d-d8b2-4eb8-bcd8-1d3248fc9a9c");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "a9de7b8a-9ca0-45ab-80c5-8271079f07d1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9b69c184-0bad-49e3-bcd2-630f751c15e7");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "71d90607-355f-4874-8d72-91dcbc49526f");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "cb8a2326-6629-4a0b-8d81-4a552bbc4505");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d478d36c-0d1f-4831-9496-fdaf0d9d5db9");
        }
    }
}
