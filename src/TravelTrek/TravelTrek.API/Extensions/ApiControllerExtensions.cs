using Microsoft.AspNetCore.Mvc;

namespace TravelTrek.API.Extensions
{
    public static class ApiControllerExtensions
    {
        public static IServiceCollection AddApiControllers(this IServiceCollection services)
        {
            services.AddControllers()
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
                            type = "Validation",
                            timestamp = DateTime.UtcNow,
                            errors
                        };

                        return new BadRequestObjectResult(response);
                    };
                });

            return services;
        }
    }
}
