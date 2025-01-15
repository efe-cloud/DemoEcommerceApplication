﻿// <auto-generated />
using ECommerce.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ECommerce.Api.Migrations
{
    [DbContext(typeof(AppDbScope))]
    [Migration("20250104131450_InitLowercaseTables")]
    partial class InitLowercaseTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.library.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("categories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Image = "electronics.png",
                            Name = "electronics"
                        },
                        new
                        {
                            Id = 2,
                            Image = "clothing.png",
                            Name = "clothing"
                        },
                        new
                        {
                            Id = 3,
                            Image = "home_kitchen.png",
                            Name = "home & kitchen"
                        },
                        new
                        {
                            Id = 4,
                            Image = "books.png",
                            Name = "books"
                        },
                        new
                        {
                            Id = 5,
                            Image = "sports.png",
                            Name = "sports"
                        });
                });

            modelBuilder.Entity("Ecommerce.library.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("categoryid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("products", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Description = "Gaming laptop with RTX 3060",
                            Name = "laptop",
                            Price = 1500.0,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Description = "Latest 5G smartphone",
                            Name = "smartphone",
                            Price = 999.99000000000001,
                            Quantity = 25
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 2,
                            Description = "Cotton round neck t-shirt",
                            Name = "t-shirt",
                            Price = 19.989999999999998,
                            Quantity = 50
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 2,
                            Description = "Slim fit denim jeans",
                            Name = "jeans",
                            Price = 49.990000000000002,
                            Quantity = 30
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 3,
                            Description = "500W powerful blender",
                            Name = "blender",
                            Price = 79.989999999999995,
                            Quantity = 15
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 3,
                            Description = "Non-stick cookware set of 5",
                            Name = "cookware set",
                            Price = 120.0,
                            Quantity = 8
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 4,
                            Description = "Best-selling fiction book",
                            Name = "fiction novel",
                            Price = 14.99,
                            Quantity = 40
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 4,
                            Description = "Lined notebook for school",
                            Name = "notebook",
                            Price = 5.9900000000000002,
                            Quantity = 100
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 5,
                            Description = "Professional-grade tennis racket",
                            Name = "tennis racket",
                            Price = 129.99000000000001,
                            Quantity = 12
                        },
                        new
                        {
                            Id = 10,
                            CategoryId = 5,
                            Description = "Non-slip eco-friendly yoga mat",
                            Name = "yoga mat",
                            Price = 39.990000000000002,
                            Quantity = 20
                        });
                });

            modelBuilder.Entity("Ecommerce.library.Models.Product", b =>
                {
                    b.HasOne("Ecommerce.library.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Ecommerce.library.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
