using discipline.hangfire.account_modification.Events.External;
using discipline.hangfire.infrastructure.Messaging.RabbitMq.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class UsersServicesConfigurationExtensions
{
    public static IServiceCollection SetAccountModification(this IServiceCollection services)
        => services
            .AddDal()
            .AddAccountStrategy()
            .AddRabbitMqConsumer<AccountModified>();
}