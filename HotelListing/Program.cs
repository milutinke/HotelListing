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
                Log.Information("Application starting...");

                var builder = WebApplication.CreateBuilder(args);

                // Logging
                builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Listing", Version = "v1" });
                });

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