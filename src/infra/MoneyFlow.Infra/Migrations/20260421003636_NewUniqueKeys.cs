using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFlow.Infra.Migrations
{
    /// <inheritdoc />
    public partial class NewUniqueKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Isector2",
                schema: "app",
                table: "sectors");

            migrationBuilder.DropIndex(
                name: "imarket2",
                schema: "app",
                table: "markets");

            migrationBuilder.DropIndex(
                name: "icurrency2",
                schema: "app",
                table: "currencies");

            migrationBuilder.DropIndex(
                name: "icategory3",
                schema: "app",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "Isector2",
                schema: "app",
                table: "sectors",
                column: "name",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "imarket2",
                schema: "app",
                table: "markets",
                column: "name",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "icurrency2",
                schema: "app",
                table: "currencies",
                column: "name",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "icategory3",
                schema: "app",
                table: "categories",
                column: "name",
                unique: true,
                filter: "is_deleted = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Isector2",
                schema: "app",
                table: "sectors");

            migrationBuilder.DropIndex(
                name: "imarket2",
                schema: "app",
                table: "markets");

            migrationBuilder.DropIndex(
                name: "icurrency2",
                schema: "app",
                table: "currencies");

            migrationBuilder.DropIndex(
                name: "icategory3",
                schema: "app",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "Isector2",
                schema: "app",
                table: "sectors",
                column: "name",
                unique: true,
                filter: "IsDeleted = false");

            migrationBuilder.CreateIndex(
                name: "imarket2",
                schema: "app",
                table: "markets",
                column: "name",
                unique: true,
                filter: "IsDeleted = false");

            migrationBuilder.CreateIndex(
                name: "icurrency2",
                schema: "app",
                table: "currencies",
                column: "name",
                unique: true,
                filter: "IsDeleted = false");

            migrationBuilder.CreateIndex(
                name: "icategory3",
                schema: "app",
                table: "categories",
                column: "name",
                unique: true,
                filter: "IsDeleted = false");
        }
    }
}
