using discipline.hangfire.shared.abstractions.Events;

namespace discipline.hangfire.activity_rules.Events.External;

public sealed record ActivityRuleDeleted(string UserId, string ActivityRuleId) : IEvent;