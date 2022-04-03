using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityProject.Data.Migrations
{
    public partial class SubNumber_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniqSubNumber",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqSubNumber",
                table: "Users");
        }
    }
}
