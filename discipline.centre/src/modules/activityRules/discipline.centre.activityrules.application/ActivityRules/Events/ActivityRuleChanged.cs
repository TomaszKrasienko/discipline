using discipline.libs.events.abstractions;

namespace discipline.centre.activityrules.application.ActivityRules.Events;

public sealed record ActivityRuleChanged(
    string ActivityRuleId,
    string UserId, 
    string Title,
    string Mode,
    IReadOnlyCollection<int>? Days) : IEvent;