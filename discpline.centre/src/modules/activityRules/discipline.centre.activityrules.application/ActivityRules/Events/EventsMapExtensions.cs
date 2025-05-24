using discipline.centre.activityrules.domain.Events;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.activityrules.application.ActivityRules.Events;

/// <summary>
/// Extensions for mapping domain events on integration events
/// </summary>
internal static class EventsMapExtensions
{
    /// <summary>
    /// Maps domain events on integration events
    /// </summary>
    /// <param name="domainEvent">Domain event to be mapped</param>
    /// <returns>Instance of <see cref="IEvent"/> after mapping</returns>
    /// <exception cref="InvalidOperationException">When domain event does not exists</exception>
    internal static IEvent MapAsIntegrationEvent(this DomainEvent domainEvent) => domainEvent switch
    {
        ActivityRuleCreated @event => new ActivityRuleRegistered(
            @event.ActivityRuleId.ToString(), 
            @event.UserId.ToString(),
            @event.Details.Title,
            @event.Details.Note,
            @event.Mode.Mode.Value,
            @event.Mode.Days?.Select(x => (int)x).ToArray()),
        domain.Events.ActivityRuleChanged @event => new ActivityRuleModeChanged(
            @event.ActivityRuleId.ToString(),
            @event.UserId.ToString(),
            @event.Mode.Mode.Value,
            @event.Mode.Days?.Select(x => (int)x).ToArray()),
        _ => throw new InvalidOperationException("Unknown event type")
    };
}