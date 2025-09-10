using discipline.hangfire.shared.abstractions.ViewModels.Abstractions;

namespace discipline.hangfire.shared.abstractions.ViewModels;

public sealed record AccountViewModel : IViewModel
{
    public required string AccountId { get; init; }
}