using discipline.activity_scheduler.shared.abstractions.Identifiers;
using discipline.activity_scheduler.shared.abstractions.ViewModels.Abstractions;

namespace discipline.activity_scheduler.shared.abstractions.ViewModels;

public sealed record PlannedTaskViewModel : IViewModel
{
    public PlannedTaskId Id { get; init; }
    public ActivityRuleId ActivityRuleId { get; init; }
    public AccountId AccountId { get; init; }
    public DateOnly PlannedFor { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public bool PlannedEnable { get; init; }
    public bool ActivityCreated { get; init; }
}