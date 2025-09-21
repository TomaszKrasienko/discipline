using discipline.activity_scheduler.account_modification.Events.External;
using discipline.activity_scheduler.account_modification.Events.External.Handlers;
using discipline.libs.events.abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class UsersServicesConfigurationExtensions
{
    public static IServiceCollection SetAccountModification(this IServiceCollection services)
    {
        services
            .AddAccountStrategy()
            .AddScoped<IEventHandler<AccountModified>, AccountModifiedEventHandler>();

        services.AddConsumer<AccountModified>(sp =>
        {
            return (async (msg, ct, mt) =>
            {
                using var scope = sp.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<AccountModified>>();
                await handler.HandleAsync(msg, ct, mt);
            });
        });
        
        return services;
    }
}