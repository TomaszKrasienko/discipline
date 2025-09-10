using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.ViewModels.Abstractions;

namespace discipline.hangfire.shared.abstractions.ViewModels;

public sealed record ActivityRuleViewModel : IViewModel
{
    public required ActivityRuleId ActivityRuleId { get; init; }
    public required AccountId AccountId { get; init; }
    public string? Mode { get; init; }
    public IReadOnlyCollection<int>? SelectedDays { get; init; }
}