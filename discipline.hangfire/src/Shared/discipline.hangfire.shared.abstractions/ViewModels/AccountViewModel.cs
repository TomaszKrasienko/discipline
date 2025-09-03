using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.shared.abstractions.ViewModels;

public sealed record AccountViewModel
{
    public required AccountId AccountId { get; init; }
}