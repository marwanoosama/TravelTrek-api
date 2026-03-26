using Serilog;
using Serilog.Events;

namespace TravelTrek.API.Extensions
{
    public static class SerilogExtensions
    {
        public static IHostBuilder AddSerilogLogging(this IHostBuilder host)
        {
            host.UseSerilog((context, services, config) =>
            {
                var isDevelopment = context.HostingEnvironment.IsDevelopment();

                config
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command",
                        isDevelopment ? LogEventLevel.Information : LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
                    .WriteTo.File(
                        path: "logs/traveltrek-.log",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}");
            });

            return host;
        }
        public static WebApplication UseSerilog(this WebApplication app)
        {
            app.UseSerilogRequestLogging(opts =>
            {
                opts.GetLevel = (ctx, _, ex) =>
                    ex is not null || ctx.Response.StatusCode >= 500 ? Serilog.Events.LogEventLevel.Error :
                    ctx.Response.StatusCode >= 400 ? Serilog.Events.LogEventLevel.Warning :
                    Serilog.Events.LogEventLevel.Information;

                opts.EnrichDiagnosticContext = (diag, ctx) =>
                {
                    diag.Set("ClientIP", ctx.Connection.RemoteIpAddress?.ToString());
                    diag.Set("UserAgent", ctx.Request.Headers.UserAgent.ToString());
                };
            });

            return app;
        }
    }
}
