using discipline.libs.events.abstractions;

namespace discipline.hangfire.activity_rule_modification.Events.External;

public sealed record ActivityRuleModified(
    string ActivityRuleId,
    string UserId, 
    string? Title,
    string? Mode, 
    IReadOnlyList<int>? Days) : IEvent;