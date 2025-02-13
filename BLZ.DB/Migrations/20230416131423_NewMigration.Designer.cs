﻿// <auto-generated />
using System;
using BLZ.DB.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BLZ.DB.Migrations
{
    [DbContext(typeof(ScraperDbContext))]
    [Migration("20230416131423_NewMigration")]
    partial class NewMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GP3.Common.Models.Category", b =>
                {
                    b.Property<string>("MerchantCatId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Merchant")
                        .HasColumnType("int");

                    b.Property<string>("NameLT")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ParentCatId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Uri")
                        .HasColumnType("longtext");

                    b.HasKey("MerchantCatId");

                    b.HasIndex("ParentCatId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GP3.Common.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float?>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int?>("DiscountPrice")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountPricePerUnitOfMeasure")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<int?>("LoyaltyPrice")
                        .HasColumnType("int");

                    b.Property<int?>("LoyaltyPricePerUnitOfMeasure")
                        .HasColumnType("int");

                    b.Property<int?>("MeasureUnit")
                        .HasColumnType("int");

                    b.Property<int>("Merchant")
                        .HasColumnType("int");

                    b.Property<string>("MerchantProductId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NameEN")
                        .HasColumnType("longtext");

                    b.Property<string>("NameLT")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("PricePerUnitOfMeasure")
                        .HasColumnType("int");

                    b.Property<string>("RemappedCategoryName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("RemappedCategoryName");

                    b.HasIndex("MerchantProductId", "Merchant")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("GP3.Common.Models.Category", b =>
                {
                    b.HasOne("GP3.Common.Models.Category", "ParentCat")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCatId");

                    b.Navigation("ParentCat");
                });

            modelBuilder.Entity("GP3.Common.Models.Item", b =>
                {
                    b.HasOne("GP3.Common.Models.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("GP3.Common.Models.Category", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("SubCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
