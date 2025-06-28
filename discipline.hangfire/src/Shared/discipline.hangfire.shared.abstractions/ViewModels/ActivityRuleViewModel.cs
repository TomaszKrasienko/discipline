using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.shared.abstractions.ViewModels;

public sealed record ActivityRuleViewModel
{
    public required ActivityRuleId ActivityRuleId { get; init; }
    public required UserId UserId { get; init; }
    public string? Mode { get; init; }
    public IReadOnlyCollection<int>? SelectedDays { get; init; }
}