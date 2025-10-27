using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TideOfDestiniy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("b7d471c4-ed2b-46db-aa4d-2fa46911d22b"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("bd11cb8b-5268-474b-8220-7ec420db44d2"));

            migrationBuilder.AddColumn<bool>(
                name: "HasPurchasedGame",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PaymentOrderCode = table.Column<int>(type: "integer", nullable: false),
                    PaymentLinkId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "AuthorId", "Content", "ImageUrl", "NewsCategory", "PublishedAt", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("7f51d5ab-65fb-4608-b91d-adae10a9c991"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Nhiều tính năng mới và sửa lỗi...", null, 0, new DateTime(2025, 10, 27, 12, 34, 0, 680, DateTimeKind.Utc).AddTicks(8301), "Bản cập nhật lớn 2.5 đã ra mắt!", "2.5.0" },
                    { new Guid("ece1eceb-be7d-4b75-a14f-b8767d2ca07c"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Tham gia ngay để nhận phần thưởng hấp dẫn...", null, 1, new DateTime(2025, 10, 27, 12, 34, 0, 680, DateTimeKind.Utc).AddTicks(8304), "Sự kiện Mùa Hè Rực Lửa bắt đầu!", null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"),
                columns: new[] { "CreatedAt", "HasPurchasedGame" },
                values: new object[] { new DateTime(2025, 10, 27, 12, 34, 0, 680, DateTimeKind.Utc).AddTicks(8227), false });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                columns: new[] { "CreatedAt", "HasPurchasedGame" },
                values: new object[] { new DateTime(2025, 10, 27, 12, 34, 0, 680, DateTimeKind.Utc).AddTicks(8217), false });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("7f51d5ab-65fb-4608-b91d-adae10a9c991"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("ece1eceb-be7d-4b75-a14f-b8767d2ca07c"));

            migrationBuilder.DropColumn(
                name: "HasPurchasedGame",
                table: "Users");

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "AuthorId", "Content", "ImageUrl", "NewsCategory", "PublishedAt", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("b7d471c4-ed2b-46db-aa4d-2fa46911d22b"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Tham gia ngay để nhận phần thưởng hấp dẫn...", null, 1, new DateTime(2025, 10, 27, 3, 2, 5, 501, DateTimeKind.Utc).AddTicks(2720), "Sự kiện Mùa Hè Rực Lửa bắt đầu!", null },
                    { new Guid("bd11cb8b-5268-474b-8220-7ec420db44d2"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Nhiều tính năng mới và sửa lỗi...", null, 0, new DateTime(2025, 10, 27, 3, 2, 5, 501, DateTimeKind.Utc).AddTicks(2708), "Bản cập nhật lớn 2.5 đã ra mắt!", "2.5.0" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 3, 2, 5, 501, DateTimeKind.Utc).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 3, 2, 5, 501, DateTimeKind.Utc).AddTicks(2636));
        }
    }
}
