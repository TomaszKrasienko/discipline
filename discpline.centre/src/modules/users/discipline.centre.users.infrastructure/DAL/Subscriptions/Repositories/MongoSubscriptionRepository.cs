using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Repositories;

internal sealed class MongoSubscriptionRepository(
    UsersMongoContext context,
    IEnumerable<ISubscriptionPolicy> policies) : BaseRepository<SubscriptionDocument>(context), IReadWriteSubscriptionRepository
{
    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        var document = subscription.ToDocument();
        await AddAsync(document, cancellationToken);
    }

    public async Task<Subscription?> GetByIdAsync(SubscriptionId id, CancellationToken cancellationToken = default)
    {
        var subscriptionDocument = await GetAsync(
            x => x.Id == id.Value.ToString(),
            cancellationToken);

        if (subscriptionDocument is null)
        {
            return null;
        }

        var policy = policies.Single(
            x => x.CanByApplied(SubscriptionType.FromValue(subscriptionDocument.Type)));
        
        return subscriptionDocument.ToEntity(policy);
    }

    public Task<bool> DoesTypeExistAsync(string type, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Type == type, cancellationToken);
}