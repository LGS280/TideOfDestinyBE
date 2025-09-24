using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TideOfDestiniy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addenumcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewsCategory",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 12, 35, 59, 690, DateTimeKind.Utc).AddTicks(2393));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmailConfirmed", "LastLoginAt", "PasswordHash", "Username" },
                values: new object[] { new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), new DateTime(2025, 9, 24, 12, 35, 59, 690, DateTimeKind.Utc).AddTicks(2405), "player@gmail.com", true, null, "$2a$11$Qm/C/LMke5VZ91Ezxk73I.5dsbIqlWHrzzkG8h9f2yUZjPwIwD6ZW", "player1" });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "AuthorId", "Content", "ImageUrl", "NewsCategory", "PublishedAt", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("9dbdbeff-8a1a-4209-84dd-04024fbc91bb"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Tham gia ngay để nhận phần thưởng hấp dẫn...", null, 1, new DateTime(2025, 9, 24, 12, 35, 59, 690, DateTimeKind.Utc).AddTicks(2475), "Sự kiện Mùa Hè Rực Lửa bắt đầu!", null },
                    { new Guid("f5d1a2b7-3fd0-423c-9151-06db4291c583"), new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"), "Nhiều tính năng mới và sửa lỗi...", null, 0, new DateTime(2025, 9, 24, 12, 35, 59, 690, DateTimeKind.Utc).AddTicks(2472), "Bản cập nhật lớn 2.5 đã ra mắt!", "2.5.0" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 2, new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("9dbdbeff-8a1a-4209-84dd-04024fbc91bb"));

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: new Guid("f5d1a2b7-3fd0-423c-9151-06db4291c583"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5a"));

            migrationBuilder.DropColumn(
                name: "NewsCategory",
                table: "News");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 19, 15, 19, 27, 528, DateTimeKind.Utc).AddTicks(2618));
        }
    }
}
