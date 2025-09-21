using discipline.libs.events.abstractions;

namespace discipline.activity_scheduler.activity_rule_modification.Events.External;

public sealed record ActivityRuleModified(
    string ActivityRuleId,
    string UserId, 
    string? Title,
    string? Mode, 
    IReadOnlyList<int>? Days) : IEvent;