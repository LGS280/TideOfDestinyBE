
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.BLL.Services;
using TideOfDestiniy.DAL.Context;
using TideOfDestiniy.DAL.Interfaces;
using TideOfDestiniy.DAL.Repositories;

namespace TideOfDestiniy.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();


            // ====> ĐỊNH NGHĨA TÊN POLICY <====
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            // ===================================

            // Add services to the container.

            //CONNECT DB
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TideOfDestinyDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Đọc cấu hình JWT từ appsettings.json
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            //==================================================
            // Thêm dịch vụ xác thực
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
            //==================================================    
            //Add Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthorization, Authorization>();
            builder.Services.AddScoped<INewsService, NewsService>();
            builder.Services.AddScoped<ISystemRequirementService, SystemRequirementService>();
            builder.Services.AddScoped<IAuthService, Authorization>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();

            //Add Repositories
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<INewsRepo, NewsRepo>();
            builder.Services.AddScoped<ISystemRequirementRepo, SystemRequirementRepo>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // ====> THÊM DỊCH VỤ CORS VÀO CONTAINER <====
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()    // Cho phép TẤT CẢ các origin
                                            .AllowAnyHeader()   // Cho phép TẤT CẢ các header
                                            .AllowAnyMethod();  // Cho phép TẤT CẢ các method (GET, POST, PUT, OPTIONS...)
                                  });
            });
            // ===============================================

            // Add Swagger
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "claim-request-api", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            ApplyMigrations(app);


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

                //app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        // ====> TẠO MỘT PHƯƠNG THỨC HELPER ĐỂ CODE SẠCH SẼ HƠN <====
        private static void ApplyMigrations(IApplicationBuilder app)
        {
            // Sử dụng IServiceScopeFactory để lấy DbContext một cách an toàn
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<TideOfDestinyDbContext>();
                    // Kiểm tra xem database có thể kết nối không
                    if (context.Database.CanConnect())
                    {
                        Console.WriteLine("Applying database migrations...");
                        // Áp dụng bất kỳ migration nào chưa được áp dụng
                        context.Database.Migrate();
                        Console.WriteLine("Migrations applied successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Could not connect to the database. Skipping migrations.");
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu quá trình migrate thất bại
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database migration.");
                }
            }
        }
        // ========================================================
    }
}
