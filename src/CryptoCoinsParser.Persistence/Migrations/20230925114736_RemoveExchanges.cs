using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoCoinsParser.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_new_coin_listings_exchanges_ExchangesId",
                table: "new_coin_listings");

            migrationBuilder.DropTable(
                name: "exchanges");

            migrationBuilder.DropIndex(
                name: "IX_new_coin_listings_ExchangesId",
                table: "new_coin_listings");

            migrationBuilder.DropColumn(
                name: "ExchangesId",
                table: "new_coin_listings");

            migrationBuilder.AddColumn<string>(
                name: "Exchange",
                table: "new_coin_listings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exchange",
                table: "new_coin_listings");

            migrationBuilder.AddColumn<long>(
                name: "ExchangesId",
                table: "new_coin_listings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "exchanges",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exchanges", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_new_coin_listings_ExchangesId",
                table: "new_coin_listings",
                column: "ExchangesId");

            migrationBuilder.AddForeignKey(
                name: "FK_new_coin_listings_exchanges_ExchangesId",
                table: "new_coin_listings",
                column: "ExchangesId",
                principalTable: "exchanges",
                principalColumn: "id");
        }
    }
}
