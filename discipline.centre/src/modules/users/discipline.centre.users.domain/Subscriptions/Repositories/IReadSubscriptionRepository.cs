using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.domain.Subscriptions.Repositories;

public interface IReadSubscriptionRepository
{
    Task<Subscription?> GetByIdAsync(SubscriptionId id, CancellationToken cancellationToken = default);
    Task<bool> DoesTypeExistAsync(string type, CancellationToken cancellationToken = default);
}