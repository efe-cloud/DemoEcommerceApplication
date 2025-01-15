using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Api.Migrations
{
    /// <inheritdoc />
    public partial class notimportant5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "categoryid", "description", "image", "name", "price", "quantity" },
                values: new object[,]
                {
                    { 11, 1, "With phone call support", "smartwatch.png", "Smart Watch", 445.99000000000001, 120 },
                    { 12, 2, "Good for running", "sporthat.png", "Sport Hat", 19.989999999999998, 530 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 12);
        }
    }
}
