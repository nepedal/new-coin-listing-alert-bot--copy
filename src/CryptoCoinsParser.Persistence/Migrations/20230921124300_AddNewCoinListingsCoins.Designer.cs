﻿// <auto-generated />
using System;
using CryptoCoinsParser.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoCoinsParser.Persistence.Migrations
{
    [DbContext(typeof(TelegramBotContext))]
    [Migration("20230921124300_AddNewCoinListingsCoins")]
    partial class AddNewCoinListingsCoins
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CryptoCoinsParser.Domain.DbEntities.Coin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("coins", (string)null);
                });

            modelBuilder.Entity("CryptoCoinsParser.Domain.DbEntities.Exchanges", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("exchanges", (string)null);
                });

            modelBuilder.Entity("CryptoCoinsParser.Domain.DbEntities.NewCoinListing", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long?>("ExchangesId")
                        .HasColumnType("bigint");

                    b.Property<string>("Message")
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.HasKey("Id");

                    b.HasIndex("ExchangesId");

                    b.ToTable("new_coin_listings", (string)null);
                });

            modelBuilder.Entity("CryptoCoinsParser.Domain.DbEntities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("TelegramUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("telegram_user_id");

                    b.Property<string>("TelegramUserName")
                        .HasColumnType("text")
                        .HasColumnName("telegram_user_name");

                    b.Property<string>("TimeZoneId")
                        .HasColumnType("text")
                        .HasColumnName("time_zone_id");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("new_coin_listings_coins", b =>
                {
                    b.Property<long>("CoinsId")
                        .HasColumnType("bigint");

                    b.Property<long>("NewCoinListingsId")
                        .HasColumnType("bigint");

                    b.HasKey("CoinsId", "NewCoinListingsId");

                    b.HasIndex("NewCoinListingsId");

                    b.ToTable("new_coin_listings_coins");
                });

            modelBuilder.Entity("CryptoCoinsParser.Domain.DbEntities.NewCoinListing", b =>
                {
                    b.HasOne("CryptoCoinsParser.Domain.DbEntities.Exchanges", "Exchanges")
                        .WithMany("NewCoinListings")
                        .HasForeignKey("ExchangesId");

                    b.Navigation("Exchanges");
                });

            modelBuilder.Entity("new_coin_listings_coins", b =>
                {
                    b.HasOne("CryptoCoinsParser.Domain.DbEntities.Coin", null)
                        .WithMany()
                        .HasForeignKey("CoinsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CryptoCoinsParser.Domain.DbEntities.NewCoinListing", null)
                        .WithMany()
                        .HasForeignKey("NewCoinListingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CryptoCoinsParser.Domain.DbEntities.Exchanges", b =>
                {
                    b.Navigation("NewCoinListings");
                });
#pragma warning restore 612, 618
        }
    }
}
