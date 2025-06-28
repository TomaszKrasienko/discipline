using discipline.hangfire.shared.abstractions.Serializer;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.infrastructure.Serializer.Configuration;

internal static class SerializerServicesConfigurationExtensions
{
    internal static IServiceCollection AddSerializer(this IServiceCollection services)
        => services.AddSingleton<ISerializer, NewtonsoftJsonSerializer>();
}