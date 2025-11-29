
using FWZ.Application.Contracts;
using FWZ.Infrastructure.Configs;
using FWZ.Infrastructure.Services;

namespace FWZ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Configuring RiotApi from appsettings.json
            builder.Services.Configure<RiotApiConfig>(builder.Configuration.GetSection("RiotApi"));

            //Adding the riot client we generated in Services
            builder.Services.AddHttpClient("riot");

            //Adding dependencey inversion
            builder.Services.AddScoped<IMatchService, MatchService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            // to use wwwroot file "UI"
            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
