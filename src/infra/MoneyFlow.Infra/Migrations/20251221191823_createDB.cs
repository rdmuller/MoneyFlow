using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoneyFlow.Infra.Migrations
{
    /// <inheritdoc />
    public partial class createDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.UniqueConstraint("ak_categories_external_id", x => x.external_id);
                });

            migrationBuilder.CreateTable(
                name: "currencies",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currencies", x => x.id);
                    table.UniqueConstraint("ak_currencies_external_id", x => x.external_id);
                });

            migrationBuilder.CreateTable(
                name: "markets",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_markets", x => x.id);
                    table.UniqueConstraint("ak_markets_external_id", x => x.external_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.UniqueConstraint("ak_users_external_id", x => x.external_id);
                });

            migrationBuilder.CreateTable(
                name: "wallets",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wallets", x => x.id);
                    table.UniqueConstraint("ak_wallets_external_id", x => x.external_id);
                });

            migrationBuilder.CreateTable(
                name: "sectors",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sectors", x => x.id);
                    table.UniqueConstraint("ak_sectors_external_id", x => x.external_id);
                    table.ForeignKey(
                        name: "fk_sectors_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "app",
                        principalTable: "categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "assets",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ticker = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    sector_id = table.Column<long>(type: "bigint", nullable: false),
                    wallet_id = table.Column<long>(type: "bigint", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assets", x => x.id);
                    table.UniqueConstraint("ak_assets_external_id", x => x.external_id);
                    table.CheckConstraint("ck_ticker_with_empty_space", "rtrim(Ticker)=Ticker");
                    table.ForeignKey(
                        name: "fk_assets_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "app",
                        principalTable: "categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_assets_sectors_sector_id",
                        column: x => x.sector_id,
                        principalSchema: "app",
                        principalTable: "sectors",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_assets_wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalSchema: "app",
                        principalTable: "wallets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "iasset1",
                schema: "app",
                table: "assets",
                columns: new[] { "tenant_id", "ticker" },
                unique: true,
                filter: "Ticker is not null and Ticker <> ''");

            migrationBuilder.CreateIndex(
                name: "iasset2",
                schema: "app",
                table: "assets",
                columns: new[] { "tenant_id", "name" });

            migrationBuilder.CreateIndex(
                name: "iasset3",
                schema: "app",
                table: "assets",
                columns: new[] { "tenant_id", "category_id" });

            migrationBuilder.CreateIndex(
                name: "ix_assets_category_id",
                schema: "app",
                table: "assets",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_sector_id",
                schema: "app",
                table: "assets",
                column: "sector_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_wallet_id",
                schema: "app",
                table: "assets",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "icategory3",
                schema: "app",
                table: "categories",
                column: "name",
                unique: true,
                filter: "Active = true");

            migrationBuilder.CreateIndex(
                name: "icurrency2",
                schema: "app",
                table: "currencies",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "imarket1",
                schema: "app",
                table: "markets",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Isector2",
                schema: "app",
                table: "sectors",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_sectors_category_id",
                schema: "app",
                table: "sectors",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "iuser1",
                schema: "app",
                table: "users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "iwallet1",
                schema: "app",
                table: "wallets",
                columns: new[] { "tenant_id", "name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assets",
                schema: "app");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "app");

            migrationBuilder.DropTable(
                name: "markets",
                schema: "app");

            migrationBuilder.DropTable(
                name: "users",
                schema: "app");

            migrationBuilder.DropTable(
                name: "sectors",
                schema: "app");

            migrationBuilder.DropTable(
                name: "wallets",
                schema: "app");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "app");
        }
    }
}
