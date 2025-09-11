
using Microsoft.EntityFrameworkCore;
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
            var builder = WebApplication.CreateBuilder(args);

            // ====> ĐỊNH NGHĨA TÊN POLICY <====
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            // ===================================

            // Add services to the container.

            //CONNECT DB
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TideOfDestinyDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


            //Add Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthorization, Authorization>();
            //Add Repositories
            builder.Services.AddScoped<IUserRepo, UserRepo>();


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
                                      if (!string.IsNullOrEmpty(origins))
                                      {
                                          policy.WithOrigins(origins.Split(',')) // Tách chuỗi thành mảng các origin
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                                      }
                                  });
            });
            // ===============================================


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
