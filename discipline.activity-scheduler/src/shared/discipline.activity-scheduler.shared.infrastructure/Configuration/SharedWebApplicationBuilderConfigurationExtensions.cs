using Microsoft.AspNetCore.Builder;

namespace discipline.activity_scheduler.shared.infrastructure.Configuration;

public static class SharedWebApplicationBuilderConfigurationExtensions
{
    public static WebApplicationBuilder UseInfrastructure(this WebApplicationBuilder app)
        => app.UseLogging();
}