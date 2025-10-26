using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFlow.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddMarketUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IMarket2",
                schema: "app",
                table: "Markets");

            migrationBuilder.CreateIndex(
                name: "IMarket1",
                schema: "app",
                table: "Markets",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IMarket1",
                schema: "app",
                table: "Markets");

            migrationBuilder.CreateIndex(
                name: "IMarket2",
                schema: "app",
                table: "Markets",
                column: "Name");
        }
    }
}
