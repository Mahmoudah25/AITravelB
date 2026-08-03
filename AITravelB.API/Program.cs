
using AITravelB.Application.Common.Interfaces;
using AITravelB.Infrastructure.Service;

namespace AITravelB.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


            // Service
            builder.Services.AddHttpClient<IPlacesService, OsmPlacesService>(client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "AITravelB/1.0 (contact: your-email@example.com)");
            });
            builder.Services.AddOpenApi();

            var app = builder.Build();


            app.MapGet("/test-places", async (IPlacesService placesService) =>
            {
                var results = await placesService.SearchPlacesAsync("Istanbul", "restaurant");
                return Results.Ok(results);
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
