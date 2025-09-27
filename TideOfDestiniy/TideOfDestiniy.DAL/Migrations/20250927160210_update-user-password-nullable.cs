using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TideOfDestiniy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateuserpasswordnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("9dbdbeff-8a1a-4209-84dd-04024fbc91bb"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("f5d1a2b7-3fd0-423c-9151-06db4291c583"));

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "AuthorId", "Content", "ImageUrl", "NewsCategory", "PublishedAt", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("57fd4eb0-0e2f-49c1-af4e-7864dee3acd9"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Tham gia ngay để nhận phần thưởng hấp dẫn...", null, 1, new DateTime(2025, 9, 27, 16, 2, 10, 10, DateTimeKind.Utc).AddTicks(7120), "Sự kiện Mùa Hè Rực Lửa bắt đầu!", null },
                    { new Guid("e1be7317-abdf-4a6a-89d9-f0fecbf8db4c"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Nhiều tính năng mới và sửa lỗi...", null, 0, new DateTime(2025, 9, 27, 16, 2, 10, 10, DateTimeKind.Utc).AddTicks(7117), "Bản cập nhật lớn 2.5 đã ra mắt!", "2.5.0" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 16, 2, 10, 10, DateTimeKind.Utc).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 16, 2, 10, 10, DateTimeKind.Utc).AddTicks(7040));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("57fd4eb0-0e2f-49c1-af4e-7864dee3acd9"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("e1be7317-abdf-4a6a-89d9-f0fecbf8db4c"));

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "AuthorId", "Content", "ImageUrl", "NewsCategory", "PublishedAt", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("9dbdbeff-8a1a-4209-84dd-04024fbc91bb"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Tham gia ngay để nhận phần thưởng hấp dẫn...", null, 1, new DateTime(2025, 9, 24, 12, 35, 59, 690, DateTimeKind.Utc).AddTicks(2475), "Sự kiện Mùa Hè Rực Lửa bắt đầu!", null },
                    { new Guid("f5d1a2b7-3fd0-423c-9151-06db4291c583"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Nhiều tính năng mới và sửa lỗi...", null, 0, new DateTime(2025, 9, 24, 12, 35, 59, 690, DateTimeKind.Utc).AddTicks(2472), "Bản cập nhật lớn 2.5 đã ra mắt!", "2.5.0" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 12, 35, 59, 690, DateTimeKind.Utc).AddTicks(2405));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 12, 35, 59, 690, DateTimeKind.Utc).AddTicks(2393));
        }
    }
}
