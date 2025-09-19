using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.DAL.Context
{
    public class TideOfDestinyDbContext : DbContext
    {
        public TideOfDestinyDbContext(DbContextOptions<TideOfDestinyDbContext> options) : base(options){ }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<SystemRequirement> SystemRequirements { get; set; }
        public DbSet<GameBuild> GameBuilds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình khóa chính phức hợp cho bảng UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Cấu hình mối quan hệ nhiều-nhiều giữa User và Role
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Cấu hình các ràng buộc UNIQUE
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            // Cấu hình để Enum được lưu dưới dạng chuỗi trong DB (dễ đọc hơn)
            modelBuilder.Entity<SystemRequirement>()
                .Property(s => s.Type)
                .HasConversion<string>();

            modelBuilder.Entity<GameBuild>()
                .Property(b => b.Platform)
                .HasConversion<string>();


            const int ADMIN_ROLE_ID = 1;
            const string PLAYER_ROLE_ID = "f1e2d3c4-b5a6-4f7e-8d9c-0a1b2c3d4e5f";
            const string ADMIN_USER_ID = "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f";

            // (Tùy chọn) Thêm dữ liệu mẫu (Seed data)
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "Player" }
            );

            var adminUser = new User
            {
                Id = new Guid(ADMIN_USER_ID),
                Username = "admin",
                Email = "admin@tideofdestiny.com", // Email của admin
                EmailConfirmed = true,
                // DÁN CHUỖI HASH BẠN ĐÃ TẠO Ở BƯỚC 1 VÀO ĐÂY
                PasswordHash = "$2a$11$Qm/C/LMke5VZ91Ezxk73I.5dsbIqlWHrzzkG8h9f2yUZjPwIwD6ZW"
            };
            modelBuilder.Entity<User>().HasData(adminUser);

            // 4. Liên kết Admin User với Admin Role
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    UserId = new Guid(ADMIN_USER_ID),
                    RoleId = ADMIN_ROLE_ID
                }
            );

        }
    }

}

