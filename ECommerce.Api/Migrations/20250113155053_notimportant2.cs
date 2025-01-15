using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Api.Migrations
{
    /// <inheritdoc />
    public partial class notimportant2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "categoryid", "description", "image", "name", "price", "quantity" },
                values: new object[,]
                {
                    { 34, 3, "tasty and does not include added sugar", "dubaichocolate.png", "Dubai Chocolate", 5.9900000000000002, 220 },
                    { 37, 1, "store your things in it efficently", "usb128gb.png", "usb128gb", 9.9900000000000002, 230 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 37);
        }
    }
}
