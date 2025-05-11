namespace discipline.hangfire.shared.abstractions.ViewModels;

public sealed record PlannedTaskDetailsViewModel(
    string ActivityRuleId,
    DateOnly PlannedFor, 
    DateTimeOffset CreatedAt,
    bool IsPlannedEnabled,
    bool IsActivityCreated);