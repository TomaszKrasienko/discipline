using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.browse_planned.ViewModels;

internal sealed record PlannedTaskViewModel(
    ActivityRuleId ActivityRuleId,
    DateOnly PlannedFor, 
    DateTimeOffset CreatedAt,
    bool IsPlannedEnabled,
    bool IsActivityCreated,
    AccountId AccountId);