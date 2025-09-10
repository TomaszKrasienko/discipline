using discipline.hangfire.account_modification.Events.External;
using discipline.libs.events.abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class UsersServicesConfigurationExtensions
{
    public static IServiceCollection SetAccountModification(this IServiceCollection services)
    {
        services
            .AddAccountStrategy();

        services.AddConsumer<AccountModified>(sp =>
        {
            return (async (msg, ct, mt) =>
            {
                using var scope = sp.CreateScope();
                var dispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();
                await dispatcher.HandleAsync(msg, ct, mt);
            });
        });
        
        return services;
    }
}