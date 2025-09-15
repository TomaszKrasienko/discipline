using discipline.activity_scheduler.account_modification.Strategies.Abstractions;
using discipline.activity_scheduler.shared.abstractions.Exceptions;
using discipline.libs.events.abstractions;
using Microsoft.Extensions.Logging;

namespace discipline.activity_scheduler.account_modification.Events.External.Handlers;

internal sealed class AccountModifiedEventHandler(
    ILogger<AccountModifiedEventHandler> logger,
    IEnumerable<IAccountHandlingStrategy> strategies) : IEventHandler<AccountModified>
{
    public async Task HandleAsync(
        AccountModified @event,
        CancellationToken cancellationToken,
        string? messageType = null)
    {
        if (messageType is null)
        {
            throw new ParameterNullException("AccountModifiedEventHandler.MessageType.Null");
        }
        
        var strategy = strategies.SingleOrDefault(x => x.CanBeApplied(messageType));

        if (strategy is null)
        {
            throw new InvalidArgumentException("AccountModifiedEventHandler.Strategy.Null", messageType);
        }
        
        logger.LogInformation($"Handling {strategy.GetType()} account event handler strategy");
        await strategy.HandleAsync(@event, cancellationToken);
    }
}