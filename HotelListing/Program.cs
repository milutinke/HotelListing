using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

namespace HotelListing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Db Connection
                builder.Services.AddDbContext<DatabaseContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"))
                );

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Listing", Version = "v1" });
                });

                // Logging
                builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

                Log.Information("Appliction Is Starting");

                var app = builder.Build();

                // CORS
                app.UseCors("AllowAll");

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();

                Log.Information("Connection String: " + builder.Configuration.GetConnectionString("sqlConnection"));
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Application Falied to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}