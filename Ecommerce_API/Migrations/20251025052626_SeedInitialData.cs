using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6799), "system", null, false, "system", null, "Men" },
                    { 2, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6811), "system", null, false, "system", null, "Women" },
                    { 3, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6813), "system", null, false, "system", null, "Kids" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedBy", "CreatedOn", "CurrentStock", "DeletedBy", "DeletedOn", "Description", "InStock", "IsActive", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6959), 50, "system", null, "Classic Blue Denim Jacket", true, true, false, "system", null, "Denim Jacket", 2499m },
                    { 2, 2, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6962), 80, "system", null, "Unisex Red Hoodie", true, true, false, "system", null, "Red Hoodie", 1799m },
                    { 3, 3, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6964), 100, "system", null, "Soft cotton T-shirt for kids", true, true, false, "system", null, "Kids Cartoon Tee", 599m }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "ImageUrl", "IsDeleted", "IsMain", "ModifiedBy", "ModifiedOn", "ProductId", "PublicId" },
                values: new object[,]
                {
                    { 1, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6980), "system", null, "https://via.placeholder.com/300", false, true, "system", null, 1, "demo1" },
                    { 2, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6982), "system", null, "https://via.placeholder.com/300", false, true, "system", null, 2, "demo2" },
                    { 3, "system", new DateTime(2025, 10, 25, 10, 56, 23, 924, DateTimeKind.Local).AddTicks(6983), "system", null, "https://via.placeholder.com/300", false, true, "system", null, 3, "demo3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
