using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TideOfDestiniy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateuserpasswordhashnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("57fd4eb0-0e2f-49c1-af4e-7864dee3acd9"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("e1be7317-abdf-4a6a-89d9-f0fecbf8db4c"));

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "AuthorId", "Content", "ImageUrl", "NewsCategory", "PublishedAt", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("2100788f-4caf-4cf8-a76d-6a7105502304"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Tham gia ngay để nhận phần thưởng hấp dẫn...", null, 1, new DateTime(2025, 9, 27, 16, 5, 31, 868, DateTimeKind.Utc).AddTicks(6046), "Sự kiện Mùa Hè Rực Lửa bắt đầu!", null },
                    { new Guid("7cf5e7bc-ac7b-46c9-8aae-94801d653d21"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Nhiều tính năng mới và sửa lỗi...", null, 0, new DateTime(2025, 9, 27, 16, 5, 31, 868, DateTimeKind.Utc).AddTicks(6043), "Bản cập nhật lớn 2.5 đã ra mắt!", "2.5.0" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 16, 5, 31, 868, DateTimeKind.Utc).AddTicks(5983));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 16, 5, 31, 868, DateTimeKind.Utc).AddTicks(5961));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("2100788f-4caf-4cf8-a76d-6a7105502304"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("7cf5e7bc-ac7b-46c9-8aae-94801d653d21"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "PasswordHash",
                keyValue: null,
                column: "PasswordHash",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
    }
}
