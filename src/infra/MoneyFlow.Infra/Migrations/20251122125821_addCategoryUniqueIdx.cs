using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFlow.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addCategoryUniqueIdx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ICategory2",
                schema: "app",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "ICategory3",
                schema: "app",
                table: "Categories",
                column: "Name",
                unique: true,
                filter: "Active = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ICategory3",
                schema: "app",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "ICategory2",
                schema: "app",
                table: "Categories",
                column: "Name");
        }
    }
}
