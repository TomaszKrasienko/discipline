using discipline.libs.events.abstractions;

namespace discipline.centre.activityrules.application.ActivityRules.Events;

public sealed record ActivityRuleRegistered(
    string ActivityRuleId,
    string UserId,
    string Title,
    string? Note,
    string Mode,
    IReadOnlyCollection<int>? Days) : IEvent;