using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TravelTrek.API.Middleware;
using TravelTrek.Infrastructure;
using TravelTrek.Infrastructure.Data;

namespace TravelTrek.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value?.Errors.Count > 0)
                            .SelectMany(e => e.Value!.Errors.Select(x => new
                            {
                                code = $"Validation.{e.Key}",
                                message = x.ErrorMessage
                            }))
                            .ToArray();

                        var response = new
                        {
                            code = "Validation.Failed",
                            message = "One or more validation errors occurred",
                            type = "Validation",
                            timestamp = DateTime.UtcNow,
                            errors
                        };

                        return new BadRequestObjectResult(response);
                    };
                });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TravelTrek API",
                    Version = "v1",
                    Description = "AI-powered travel planning API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token below.\r\nExample: eyJhbGci..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.MigrateAsync();
            }

            app.UseExceptionHandler();

            // Enable Swagger in Development only
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelTrek API v1");
                    options.RoutePrefix = "swagger"; // ← هنا التغيير
                    options.DisplayRequestDuration();
                    options.EnableDeepLinking();
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}