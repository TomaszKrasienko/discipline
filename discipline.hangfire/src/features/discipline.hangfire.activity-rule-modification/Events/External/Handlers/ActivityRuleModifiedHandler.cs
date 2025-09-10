using discipline.hangfire.activity_rule_modification.Strategies.Abstractions;
using discipline.libs.events.abstractions;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rule_modification.Events.External.Handlers;

internal sealed class ActivityRuleModifiedHandler(
    ILogger<ActivityRuleModifiedHandler> logger,
    IEnumerable<IActivityRuleHandlingStrategy> strategies) : IEventHandler<ActivityRuleModified>
{
    public async Task HandleAsync(
        ActivityRuleModified @event, 
        CancellationToken cancellationToken, 
        string? messageType = null)
    {
        if (messageType is null)
        {
            throw new ArgumentNullException(nameof(messageType));
        }
        
        var strategy = strategies.SingleOrDefault(x => x.CanBeApplied(messageType));

        if (strategy is null)
        {
            throw new InvalidOperationException($"No strategy found for {messageType}");
        }
        
        await strategy.HandleAsync(@event, cancellationToken);
    }
}