using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFlow.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "iasset1",
                schema: "app",
                table: "assets");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_on",
                schema: "app",
                table: "wallets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "app",
                table: "wallets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_on",
                schema: "app",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "app",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_on",
                schema: "app",
                table: "sectors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "app",
                table: "sectors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_on",
                schema: "app",
                table: "markets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "app",
                table: "markets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_on",
                schema: "app",
                table: "currencies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "app",
                table: "currencies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_on",
                schema: "app",
                table: "categories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "app",
                table: "categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_on",
                schema: "app",
                table: "assets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "app",
                table: "assets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "iasset1",
                schema: "app",
                table: "assets",
                columns: new[] { "tenant_id", "ticker" },
                filter: "Ticker is not null and Ticker <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "iasset1",
                schema: "app",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                schema: "app",
                table: "wallets");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "app",
                table: "wallets");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                schema: "app",
                table: "users");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "app",
                table: "users");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                schema: "app",
                table: "sectors");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "app",
                table: "sectors");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                schema: "app",
                table: "markets");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "app",
                table: "markets");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                schema: "app",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "app",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                schema: "app",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "app",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                schema: "app",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "app",
                table: "assets");

            migrationBuilder.CreateIndex(
                name: "iasset1",
                schema: "app",
                table: "assets",
                columns: new[] { "tenant_id", "ticker" },
                unique: true,
                filter: "Ticker is not null and Ticker <> ''");
        }
    }
}
