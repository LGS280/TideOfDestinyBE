using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TideOfDestiniy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seedinguserroleadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmailConfirmed", "LastLoginAt", "PasswordHash", "Username" },
                values: new object[] { new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"), new DateTime(2025, 9, 19, 15, 19, 27, 528, DateTimeKind.Utc).AddTicks(2618), "admin@tideofdestiny.com", true, null, "$2a$11$Qm/C/LMke5VZ91Ezxk73I.5dsbIqlWHrzzkG8h9f2yUZjPwIwD6ZW", "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f"));
        }
    }
}
