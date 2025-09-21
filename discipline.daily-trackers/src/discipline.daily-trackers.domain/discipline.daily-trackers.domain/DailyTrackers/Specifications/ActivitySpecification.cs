using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers.Specifications;

public sealed record ActivitySpecification(
    ActivityId ActivityId,
    ActivityDetailsSpecification Details,
    ActivityRuleId ParentActivityRuleId,
    IReadOnlyCollection<StageSpecification> Stages);