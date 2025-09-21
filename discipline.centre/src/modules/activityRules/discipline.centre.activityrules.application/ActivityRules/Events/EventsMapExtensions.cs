using discipline.centre.activityrules.domain.Events;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.libs.events.abstractions;

namespace discipline.centre.activityrules.application.ActivityRules.Events;

internal static class EventsMapExtensions
{
    internal static IEvent MapAsIntegrationEvent(this DomainEvent domainEvent) => domainEvent switch
    {
        ActivityRuleCreated @event => new ActivityRuleRegistered(
            @event.ActivityRuleId.ToString(), 
            @event.AccountId.ToString(),
            @event.Details.Title,
            @event.Details.Note,
            @event.Mode.Mode.Value,
            @event.Mode.Days?.Select(x => (int)x).ToArray()),
        domain.Events.ActivityRuleChanged @event => new ActivityRuleChanged(
            @event.ActivityRuleId.ToString(),
            @event.AccountId.ToString(),
            @event.Details.Title,
            @event.Mode.Mode.Value,
            @event.Mode.Days?.Select(x => (int)x).ToArray()),
        _ => throw new InvalidOperationException("Unknown event type")
    };
}