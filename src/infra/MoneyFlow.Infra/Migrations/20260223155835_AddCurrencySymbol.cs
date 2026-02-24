using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFlow.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencySymbol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "symbol",
                schema: "app",
                table: "currencies",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "symbol",
                schema: "app",
                table: "currencies");
        }
    }
}
