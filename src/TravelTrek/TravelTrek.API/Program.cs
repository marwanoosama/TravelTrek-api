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

            // Add services to the container.
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Override default 400 response from [ApiController] to match our Result pattern
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value?.Errors.Count > 0)
                            .SelectMany(e => e.Value!.Errors.Select(x =>
                                new { field = e.Key, message = x.ErrorMessage }))
                            .ToList();

                        return new BadRequestObjectResult(new
                        {
                            error = "One or more validation errors occurred.",
                            details = errors
                        });
                    };
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Global Exception Handler (.NET 8 IExceptionHandler)
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            // Infrastructure
            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            //pending migrations
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.MigrateAsync();
            }


            // Must be first in the pipeline to catch all exceptions
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
