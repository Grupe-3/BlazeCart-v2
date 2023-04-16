﻿// <auto-generated />
using System;
using BLZ.DB.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GP3.DB.Migrations
{
    [DbContext(typeof(ScraperDbContext))]
    partial class ScraperDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BLZ.Common.Models.Category", b =>
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

            modelBuilder.Entity("BLZ.Common.Models.Item", b =>
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

            modelBuilder.Entity("BLZ.Common.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Reason")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("BLZ.Common.Models.Category", b =>
                {
                    b.HasOne("BLZ.Common.Models.Category", "ParentCat")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCatId");

                    b.Navigation("ParentCat");
                });

            modelBuilder.Entity("BLZ.Common.Models.Item", b =>
                {
                    b.HasOne("BLZ.Common.Models.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BLZ.Common.Models.Report", b =>
                {
                    b.HasOne("BLZ.Common.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("BLZ.Common.Models.Category", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("SubCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
