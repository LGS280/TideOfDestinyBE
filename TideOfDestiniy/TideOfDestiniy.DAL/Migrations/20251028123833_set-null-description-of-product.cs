using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TideOfDestiniy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class setnulldescriptionofproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("7f51d5ab-65fb-4608-b91d-adae10a9c991"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("ece1eceb-be7d-4b75-a14f-b8767d2ca07c"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "AuthorId", "Content", "ImageUrl", "NewsCategory", "PublishedAt", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("16793a02-9f01-47d7-882c-768ed3a4b3b0"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Tham gia ngay để nhận phần thưởng hấp dẫn...", null, 1, new DateTime(2025, 10, 28, 12, 38, 33, 382, DateTimeKind.Utc).AddTicks(712), "Sự kiện Mùa Hè Rực Lửa bắt đầu!", null },
                    { new Guid("8ec05a34-af0f-4c56-9fe0-557737784917"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Nhiều tính năng mới và sửa lỗi...", null, 0, new DateTime(2025, 10, 28, 12, 38, 33, 382, DateTimeKind.Utc).AddTicks(709), "Bản cập nhật lớn 2.5 đã ra mắt!", "2.5.0" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 12, 38, 33, 382, DateTimeKind.Utc).AddTicks(641));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 12, 38, 33, 382, DateTimeKind.Utc).AddTicks(630));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("16793a02-9f01-47d7-882c-768ed3a4b3b0"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("8ec05a34-af0f-4c56-9fe0-557737784917"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

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
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 12, 34, 0, 680, DateTimeKind.Utc).AddTicks(8227));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 12, 34, 0, 680, DateTimeKind.Utc).AddTicks(8217));
        }
    }
}
