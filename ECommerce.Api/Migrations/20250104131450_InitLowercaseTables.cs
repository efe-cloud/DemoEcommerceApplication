using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitLowercaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    categoryid = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_categories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "image", "name" },
                values: new object[,]
                {
                    { 1, "electronics.png", "electronics" },
                    { 2, "clothing.png", "clothing" },
                    { 3, "home_kitchen.png", "home & kitchen" },
                    { 4, "books.png", "books" },
                    { 5, "sports.png", "sports" }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "categoryid", "description", "name", "price", "quantity" },
                values: new object[,]
                {
                    { 1, 1, "Gaming laptop with RTX 3060", "laptop", 1500.0, 10 },
                    { 2, 1, "Latest 5G smartphone", "smartphone", 999.99000000000001, 25 },
                    { 3, 2, "Cotton round neck t-shirt", "t-shirt", 19.989999999999998, 50 },
                    { 4, 2, "Slim fit denim jeans", "jeans", 49.990000000000002, 30 },
                    { 5, 3, "500W powerful blender", "blender", 79.989999999999995, 15 },
                    { 6, 3, "Non-stick cookware set of 5", "cookware set", 120.0, 8 },
                    { 7, 4, "Best-selling fiction book", "fiction novel", 14.99, 40 },
                    { 8, 4, "Lined notebook for school", "notebook", 5.9900000000000002, 100 },
                    { 9, 5, "Professional-grade tennis racket", "tennis racket", 129.99000000000001, 12 },
                    { 10, 5, "Non-slip eco-friendly yoga mat", "yoga mat", 39.990000000000002, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_categoryid",
                table: "products",
                column: "categoryid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
