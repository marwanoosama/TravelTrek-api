using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace TravelTrek.API.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddAuthRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        code = "RateLimit.Exceeded",
                        type = "RateLimit",
                        message = "Too many requests. Please slow down and try again later.",
                        timestamp = DateTime.UtcNow
                    }, cancellationToken);
                };

                static RateLimitPartition<string> SlidingWindow(
                    string key, int permitLimit, int windowSeconds, int segments) =>
                    RateLimitPartition.GetSlidingWindowLimiter(key, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        Window = TimeSpan.FromSeconds(windowSeconds),
                        SegmentsPerWindow = segments,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });

                // 10 req / 1 min
                options.AddPolicy("auth-login", context =>
                    SlidingWindow(context.Connection.RemoteIpAddress?.ToString() ?? "unknown", 10, 60, 2));

                // 5 req / 1 min
                options.AddPolicy("auth-register", context =>
                    SlidingWindow(context.Connection.RemoteIpAddress?.ToString() ?? "unknown", 5, 60, 2));

                // 10 req / 1 min
                options.AddPolicy("auth-google", context =>
                    SlidingWindow(context.Connection.RemoteIpAddress?.ToString() ?? "unknown", 10, 60, 2));

                // 20 req / 1 min
                options.AddPolicy("auth-refresh", context =>
                    SlidingWindow(context.Connection.RemoteIpAddress?.ToString() ?? "unknown", 20, 60, 2));

                // 10 req / 1 min
                options.AddPolicy("auth-revoke", context =>
                    SlidingWindow(context.Connection.RemoteIpAddress?.ToString() ?? "unknown", 10, 60, 2));
            });

            return services;
        }
    }
}
