
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using TideOfDestiniy.BLL.Hubs;
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

            // ====> ĐỊNH NGHĨA TÊN POLICY <====
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            // ===================================

            // Add services to the container.

            //CONNECT DB
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TideOfDestinyDbContext>(options =>
                options.UseNpgsql(connectionString));

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
            builder.Services.AddScoped<IAuthService, Authorization>();
            builder.Services.AddScoped<INewsService, NewsService>();
            builder.Services.AddScoped<ISystemRequirementService, SystemRequirementService>();
            builder.Services.AddScoped<IUploadService ,UploadService>();
            builder.Services.AddScoped<IDownloadGameService, DownloadGameService>();
            builder.Services.AddScoped<IR2StorageService, R2StorageService>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IEmailService, EmailService>();


            //Add Repositories
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<INewsRepo, NewsRepo>();
            builder.Services.AddScoped<ISystemRequirementRepo, SystemRequirementRepo>();
            builder.Services.AddScoped<IFileRepo, FileRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<IPasswordResetTokenRepo, PasswordResetTokenRepo>();

            builder.Services.AddHttpClient();

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
                                      var origins = builder.Configuration.GetValue<string>("CorsOrigins");
                                      var allowedOrigins = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                                      
                                      // Always include the production domain - this is critical
                                      var productionDomain = "https://tide-of-destiny-client.vercel.app";
                                      allowedOrigins.Add(productionDomain);
                                      
                                      // Add origins from configuration (avoid duplicates)
                                      if (!string.IsNullOrEmpty(origins))
                                      {
                                          var configOrigins = origins.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                              .Select(o => o.Trim())
                                              .Where(o => !string.IsNullOrEmpty(o));
                                          foreach (var origin in configOrigins)
                                          {
                                              allowedOrigins.Add(origin);
                                          }
                                      }
                                      
                                      // Convert to array for WithOrigins (most reliable method with AllowCredentials)
                                      var originsArray = allowedOrigins.ToArray();
                                      
                                      if (originsArray.Length > 0)
                                      {
                                          // Use WithOrigins directly - most reliable with AllowCredentials
                                          policy.WithOrigins(originsArray)
                                                .AllowAnyHeader()
                                                .AllowAnyMethod()
                                                .AllowCredentials() // Required for cookies/auth
                                                .SetPreflightMaxAge(TimeSpan.FromHours(1));
                                      }
                                      else
                                      {
                                          // Fallback - should not happen but safety net
                                          policy.AllowAnyOrigin()
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                                      }
                                  });
                
                // Add a separate policy for Swagger UI same-origin requests
                options.AddPolicy("AllowAll",
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
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

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue; // gần như không giới hạn
            });
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = long.MaxValue; // hoặc set giá trị cụ thể ví dụ 5GB
            });
            var r2Config = builder.Configuration.GetSection("R2Storage");

            builder.Services.AddSingleton<IAmazonS3>(sp =>
            {
                return new AmazonS3Client(
                    r2Config["AccessKeyId"],
                    r2Config["SecretAccessKey"],
                    new AmazonS3Config
                    {
                        ServiceURL = r2Config["AccountUrl"], // dạng: https://<accountid>.r2.cloudflarestorage.com
                        ForcePathStyle = true, // bắt buộc cho R2
                        AuthenticationRegion = "auto", // R2 yêu cầu để auto detect
                        //DisablePayloadSigning = true
                    }
                );
            });
            builder.Services.AddScoped<IR2StorageService, R2StorageService>();

            builder.Services.AddSignalR();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Lấy DbContext từ service container
                    var context = services.GetRequiredService<TideOfDestinyDbContext>();

                    // Kiểm tra xem có migration nào đang chờ được áp dụng không
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        Console.WriteLine("Applying database migrations...");
                        // Áp dụng các migration đang chờ
                        context.Database.Migrate();
                        Console.WriteLine("Database migrations applied successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Database is up to date. No migrations to apply.");
                    }
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi nếu quá trình migrate thất bại
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            // Configure the HTTP request pipeline.
            // IMPORTANT: CORS must be very early in the pipeline, before routing
            app.UseCors(MyAllowSpecificOrigins);
            
            if (app.Environment.IsDevelopment())
            {

            }
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapHub<NewsHub>("/newsHub");


            app.Run();
        }
    }
}
