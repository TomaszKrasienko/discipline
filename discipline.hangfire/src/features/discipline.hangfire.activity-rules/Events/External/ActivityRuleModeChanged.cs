using discipline.hangfire.shared.abstractions.Events;

namespace discipline.hangfire.activity_rules.Events.External;

internal sealed record ActivityRuleModeChanged(string ActivityRuleId,
    string UserId, string Mode, IReadOnlyList<int>? Days) : IEvent;