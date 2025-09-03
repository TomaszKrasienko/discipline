using System.Collections.Immutable;
using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.ViewModels;

namespace discipline.hangfire.shared.abstractions.Api;

public interface IBrowsePlannedApi
{
    Task<ImmutableDictionary<DateOnly, List<PlannedTaskDetailsViewModel>>> GetPlannedTaskDetailsAsync(
        AccountId accountId,
        CancellationToken cancellationToken = default);
}