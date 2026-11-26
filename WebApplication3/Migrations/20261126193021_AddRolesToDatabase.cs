using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a3d721b3-20ee-4ccd-9672-2f5b8225fb58", null, "User", "USER" },
                    { "d790e5dc-c547-4e89-abde-973b4c1aca51", null, "Editor", "EDITOR" },
                    { "fe3e3d30-625f-499e-8cff-8208af1dfc10", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3d721b3-20ee-4ccd-9672-2f5b8225fb58");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d790e5dc-c547-4e89-abde-973b4c1aca51");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe3e3d30-625f-499e-8cff-8208af1dfc10");
        }
    }
}
