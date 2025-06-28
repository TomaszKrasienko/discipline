using discipline.hangfire.infrastructure.RedisBroker;
using discipline.hangfire.infrastructure.RedisBroker.Configuration;
using discipline.hangfire.shared.abstractions.Brokers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class RedisBrokerServicesConfigurationExtensions
{
    internal static IServiceCollection AddRedisBroker(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<RedisBrokerOptions>(configuration.GetSection(nameof(RedisBrokerOptions)))
            .AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RedisBrokerOptions>>();
                var connectionString = options.Value.ConnectionString;

                return ConnectionMultiplexer.Connect(connectionString);
            })
            .AddScoped<IRedisClient, RedisClient>();
}