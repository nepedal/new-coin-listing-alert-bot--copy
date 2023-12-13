using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoCoinsParser.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewCoinListingsCoins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinNewCoinListing");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "new_coin_listings_coins");

            migrationBuilder.CreateTable(
                name: "CoinNewCoinListing",
                columns: table => new
                {
                    CoinsId = table.Column<long>(type: "bigint", nullable: false),
                    NewCoinListingsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinNewCoinListing", x => new { x.CoinsId, x.NewCoinListingsId });
                    table.ForeignKey(
                        name: "FK_CoinNewCoinListing_coins_CoinsId",
                        column: x => x.CoinsId,
                        principalTable: "coins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoinNewCoinListing_new_coin_listings_NewCoinListingsId",
                        column: x => x.NewCoinListingsId,
                        principalTable: "new_coin_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinNewCoinListing_NewCoinListingsId",
                table: "CoinNewCoinListing",
                column: "NewCoinListingsId");
        }
    }
}
