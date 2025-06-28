using discipline.hangfire.shared.abstractions.Events;

namespace discipline.hangfire.activity_rules.Events.External;

internal sealed record ActivityRuleRegistered(
    string ActivityRuleId,
    string UserId,
    string Title,
    string? Note,
    string Mode,
    IReadOnlyCollection<int>? Days) : IEvent;