using discipline.hangfire.shared.abstractions.Events;

namespace discipline.hangfire.activity_rules.Events.External;

internal sealed record ActivityRuleModified(
    string ActivityRuleId,
    string UserId, 
    string? Title,
    string? Mode, 
    IReadOnlyList<int>? Days) : IEvent;