using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Subscriptions.Commands.CreateSubscription;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace discipline.centre.users.infrastructure.DataInitializers;

internal sealed class SubscriptionInitializer(
    IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        List<CreateSubscriptionCommand> createSubscriptionCommands = 
            [
                new(SubscriptionId.New(), 
                    SubscriptionType.Standard, 
                    []),
                new(SubscriptionId.New(), 
                    SubscriptionType.Premium,
                    [new PriceSpecification(
                        10,100, Currency.Pln)]),
                new(SubscriptionId.New(), 
                    SubscriptionType.Admin, 
                    [])
            ];

        using var scope = serviceProvider.CreateScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<ICqrsDispatcher>();
        List<Task> creationTasks = [];
        
        foreach (var command in createSubscriptionCommands)
        {
            creationTasks.Add(dispatcher.HandleAsync(command, cancellationToken));
        }
        
        await Task.WhenAll(creationTasks);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}