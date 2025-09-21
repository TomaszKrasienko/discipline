using discipline.activity_scheduler.shared.abstractions.ViewModels.Abstractions;

namespace discipline.activity_scheduler.shared.abstractions.ViewModels;

public sealed record AccountViewModel : IViewModel
{
    public required string AccountId { get; init; }
}