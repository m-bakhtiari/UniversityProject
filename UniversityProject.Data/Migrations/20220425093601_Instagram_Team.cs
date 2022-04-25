using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityProject.Data.Migrations
{
    public partial class Instagram_Team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Teams",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "Teams",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "Teams",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "Teams");
        }
    }
}
