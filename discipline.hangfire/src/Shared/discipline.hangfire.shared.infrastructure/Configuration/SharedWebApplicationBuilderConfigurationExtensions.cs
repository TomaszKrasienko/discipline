using discipline.hangfire.infrastructure.Logging;
using Microsoft.AspNetCore.Builder;

namespace discipline.hangfire.infrastructure.Configuration;

public static class SharedWebApplicationBuilderConfigurationExtensions
{
    public static WebApplicationBuilder UseInfrastructure(this WebApplicationBuilder app)
        => app.UseLogging();
}