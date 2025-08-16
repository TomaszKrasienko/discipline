using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.Repositories;

public interface IReadActivityRuleRepository
{
    Task<bool> ExistsAsync(string title, AccountId accountId, CancellationToken cancellationToken = default);
    Task<ActivityRule?> GetByIdAsync(ActivityRuleId id, AccountId accountId, CancellationToken cancellationToken = default);
}