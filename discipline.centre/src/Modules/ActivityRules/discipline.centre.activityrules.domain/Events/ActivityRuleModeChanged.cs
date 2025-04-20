using discipline.centre.activityrules.domain.Enums;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.Events;

public sealed record ActivityRuleModeChanged(ActivityRuleId ActivityRuleId,
    UserId UserId, RuleMode RuleMode, IReadOnlySet<DayOfWeek>? Days) : DomainEvent;