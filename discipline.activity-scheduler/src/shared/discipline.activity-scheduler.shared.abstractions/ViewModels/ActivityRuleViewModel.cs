using discipline.activity_scheduler.shared.abstractions.Identifiers;
using discipline.activity_scheduler.shared.abstractions.ViewModels.Abstractions;

namespace discipline.activity_scheduler.shared.abstractions.ViewModels;

public sealed record ActivityRuleViewModel : IViewModel
{
    public required ActivityRuleId ActivityRuleId { get; init; }
    public required AccountId AccountId { get; init; }
    public string? Mode { get; init; }
    public IReadOnlyCollection<int>? SelectedDays { get; init; }
}