using discipline.centre.shared.abstractions.Events;

namespace discipline.centre.activityrules.application.ActivityRules.Events;

public sealed record ActivityRuleModeChanged(string ActivityRuleId,
    string UserId, string Mode, List<int>? Days) : IEvent;