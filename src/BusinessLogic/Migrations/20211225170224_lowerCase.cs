using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessLogic.Migrations
{
    public partial class lowerCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Polls",
                table: "Polls");

            migrationBuilder.RenameTable(
                name: "Requests",
                newName: "requests");

            migrationBuilder.RenameTable(
                name: "Polls",
                newName: "polls");

            migrationBuilder.AddPrimaryKey(
                name: "PK_requests",
                table: "requests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_polls",
                table: "polls",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_requests",
                table: "requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_polls",
                table: "polls");

            migrationBuilder.RenameTable(
                name: "requests",
                newName: "Requests");

            migrationBuilder.RenameTable(
                name: "polls",
                newName: "Polls");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Polls",
                table: "Polls",
                column: "Id");
        }
    }
}
