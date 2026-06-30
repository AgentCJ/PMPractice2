using MasterFloorAPI.Models;
using MasterFloorAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

<<<<<<< HEAD
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MasterFloorContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMaterialCalculator, MaterialCalculator>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
=======
namespace ShoesApi
{
    public class Program
>>>>>>> ca9abd8f2329556eb9a8ac67c6ffd9b0d0d912cf
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddDbContext<MasterFloorContext>(options => options.UseNpgsql(connectionString));
            builder.Services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.WebHost.UseUrls("http://localhost:5142");

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
            
    }
}
        