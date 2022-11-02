using HotelListing.Configurations;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Repository;
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

                builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

                // Auto-Mapper
                builder.Services.AddAutoMapper(typeof(MapperInitializer));

                // CORS
                builder.Services.AddCors(o =>
                {
                    o.AddPolicy("AllowAll", builder =>
                        builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                    );
                });

                builder.Services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

                // Swagger
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Listing", Version = "v1" });
                });

                // Logging
                builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

                Log.Information("Appliction Is Starting");

                var app = builder.Build();

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