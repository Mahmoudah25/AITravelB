
using AITravelB.Application.Common.Interfaces;
using AITravelB.Application.Trips.Commands;
using AITravelB.Infrastructure.ExteranlService.Gemini;
using AITravelB.Infrastructure.ExteranlService.Groq;
using AITravelB.Infrastructure.Service;
using AITravelB.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AITravelB.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

           
            builder.Services.AddControllers();

            ///////////////////////
            // DB Context
            ///////////////////////
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            //////////
            // Service
            /////////
            builder.Services.AddHttpClient<IPlacesService, OsmPlacesService>(client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "AITravelB/1.0 (contact: your-email@example.com)");
            });
            builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());
            //builder.Services.AddScoped<IItineraryAiService, GeminiItineraryService>();
            builder.Services.AddHttpClient<IItineraryAiService, GroqItineraryService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTripCommand).Assembly));
            //builder.Services.AddOpenApi();

            // =======================
            // Swagger
            // =======================
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AITripPlanner",
                    Description = "ASP.NET Core Web API"
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
            });

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
