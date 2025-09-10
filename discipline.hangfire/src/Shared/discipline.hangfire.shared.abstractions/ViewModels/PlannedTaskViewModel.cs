using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.ViewModels.Abstractions;

namespace discipline.hangfire.shared.abstractions.ViewModels;

public sealed record PlannedTaskViewModel : IViewModel
{
    public PlannedTaskId Id { get; init; }
    public ActivityRuleId ActivityRuleId { get; init; }
    public AccountId AccountId { get; init; }
    public DateOnly PlannedFor { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public bool IsPlannedEnabled { get; init; }
    public bool ActivityCreated { get; init; }
}