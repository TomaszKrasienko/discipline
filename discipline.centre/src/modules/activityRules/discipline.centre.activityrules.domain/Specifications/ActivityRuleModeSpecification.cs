using discipline.centre.activityrules.domain.Enums;

namespace discipline.centre.activityrules.domain.Specifications;

public sealed record ActivityRuleModeSpecification(RuleMode Mode, HashSet<int>? Days);