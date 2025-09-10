using discipline.libs.serializers;
using discipline.libs.serializers.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SerializationServicesConfigurationExtensions
{
    public static IServiceCollection AddSerialization(this IServiceCollection services)
        => services.AddSingleton<ISerializer, SystemTextSerializer>();
}