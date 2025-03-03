using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _NET_MinimalAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdministrator2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Administrators",
                columns: new[] { "Id", "Mail", "Password", "Profile" },
                values: new object[] { 1, "admin@test.com", "123456", "ADM" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
