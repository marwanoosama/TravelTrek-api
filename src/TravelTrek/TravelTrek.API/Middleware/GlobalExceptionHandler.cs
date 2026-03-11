using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TravelTrek.API.Middleware
{
    /// <summary>
    /// Catches any unhandled exception in the pipeline and returns a clean JSON response.
    /// Prevents stack traces and internal details from leaking to the client in production.
    /// </summary>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);

            var (statusCode, title) = exception switch
            {
                ArgumentNullException     => (StatusCodes.Status400BadRequest,  "Bad Request"),
                ArgumentException         => (StatusCodes.Status400BadRequest,  "Bad Request"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                _                         => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title  = title,
                Detail = _env.IsDevelopment()
                    ? exception.Message          // show details in Development only
                    : "An unexpected error occurred. Please try again later.",
                Extensions =
                {
                    ["traceId"] = httpContext.TraceIdentifier
                }
            };

            httpContext.Response.StatusCode  = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // true = exception was handled, don't rethrow
        }
    }
}
