using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversityProject.Data.Migrations
{
    public partial class adminData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1001,
                columns: new[] { "Password", "Phone" },
                values: new object[] { "123456", "01234567890" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1001,
                columns: new[] { "Password", "Phone" },
                values: new object[] { "1234", "No Number" });
        }
    }
}
