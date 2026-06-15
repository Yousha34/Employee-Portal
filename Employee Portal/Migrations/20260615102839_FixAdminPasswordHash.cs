using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee_Portal.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$12$vXNCUi/FeEBJs7bLMLZDwOoC629SFso6zvcecjEfVko1SFfug1rBq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$zKHiMKh4EH3Gq4G1lS5Nre7f8Y3mC6oFqP9vX2wD0uJ5tB8nR1Lm.");
        }
    }
}
