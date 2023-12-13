using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoCoinsParser.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameNewCoinListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "new_coin_listings_coins");

            migrationBuilder.DropTable(
                name: "new_coin_listings");

            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    message = table.Column<string>(type: "text", nullable: true),
                    Exchange = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "announcements_coins",
                columns: table => new
                {
                    AnnouncementsId = table.Column<long>(type: "bigint", nullable: false),
                    CoinsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcements_coins", x => new { x.AnnouncementsId, x.CoinsId });
                    table.ForeignKey(
                        name: "FK_announcements_coins_announcements_AnnouncementsId",
                        column: x => x.AnnouncementsId,
                        principalTable: "announcements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_announcements_coins_coins_CoinsId",
                        column: x => x.CoinsId,
                        principalTable: "coins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_announcements_coins_CoinsId",
                table: "announcements_coins",
                column: "CoinsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcements_coins");

            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.CreateTable(
                name: "new_coin_listings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Exchange = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_new_coin_listings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "new_coin_listings_coins",
                columns: table => new
                {
                    CoinsId = table.Column<long>(type: "bigint", nullable: false),
                    NewCoinListingsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_new_coin_listings_coins", x => new { x.CoinsId, x.NewCoinListingsId });
                    table.ForeignKey(
                        name: "FK_new_coin_listings_coins_coins_CoinsId",
                        column: x => x.CoinsId,
                        principalTable: "coins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_new_coin_listings_coins_new_coin_listings_NewCoinListingsId",
                        column: x => x.NewCoinListingsId,
                        principalTable: "new_coin_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_new_coin_listings_coins_NewCoinListingsId",
                table: "new_coin_listings_coins",
                column: "NewCoinListingsId");
        }
    }
}
