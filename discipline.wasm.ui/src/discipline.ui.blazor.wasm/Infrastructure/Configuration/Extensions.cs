using discipline.ui.communication.http;
using discipline.ui.infrastructure;

namespace discipline.ui.blazor.wasm.Infrastructure.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services
            .SetHttpCommunicationServices(configuration)
            .SetInfrastructureServices();
}