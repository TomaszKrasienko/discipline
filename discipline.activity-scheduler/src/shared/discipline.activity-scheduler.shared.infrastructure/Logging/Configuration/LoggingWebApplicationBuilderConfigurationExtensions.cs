using discipline.activity_scheduler.shared.infrastructure.Configuration.Options;
using discipline.activity_scheduler.shared.infrastructure.Logging.Configuration.Options;
using discipline.libs.configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class LoggingWebApplicationBuilderConfigurationExtensions
{
    internal static WebApplicationBuilder UseLogging(this WebApplicationBuilder app)
    {
        var seqOptions = app.Services.GetOptions<SeqOptions>();
        var appOptions = app.Services.GetOptions<AppOptions>();
        
        app.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.WithProperty("ConnectionName", appOptions.Name)
                .WriteTo.Console(
                    outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message}{NewLine}",
                    theme:AnsiConsoleTheme.Code)
                .WriteTo.Seq(seqOptions.Url);
        });

        return app;
    }
}