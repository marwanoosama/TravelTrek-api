using Microsoft.AspNetCore.Mvc;
using TravelTrek.Infrastructure;
using TravelTrek.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TravelTrek.API.Middleware;

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
            builder.Services.AddSwaggerGen();

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

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}