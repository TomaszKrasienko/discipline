using discipline.daily_trackers.infrastructure.Configuration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace discipline.daily_trackers.infrastructure.Configuration;

public static class InfrastructureWebApplicationConfigurationExtensions
{
    public static WebApplication UseInfrastructure(this WebApplication app)
        => app
            .UseAuth()
            .UseUiDocumentation();
    
    private static WebApplication UseUiDocumentation(this WebApplication app)
    {
        var appOptions = app.Services.GetRequiredService<IOptions<AppOptions>>().Value;
        
        app.UseSwagger();
        app.UseReDoc(reDoc =>
        {
            reDoc.RoutePrefix = "redoc";
            reDoc.SpecUrl("/swagger/v1/swagger.json");
            reDoc.DocumentTitle = appOptions.Name;
        });
        return app;
    }
}