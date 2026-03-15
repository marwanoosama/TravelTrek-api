using Microsoft.EntityFrameworkCore;
using TravelTrek.API.Extensions;
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

            builder.Services.AddApiControllers();
            builder.Services.AddSwaggerWithAuth();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddDistributedMemoryCache(); // will be replaced to use redis when deployment
            builder.Services.AddAuthRateLimiting();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.MigrateAsync();
            }

            app.UseExceptionHandler();
            app.UseSwaggerInDevelopment();
            app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}