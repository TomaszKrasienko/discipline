using System.Linq.Expressions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Repositories;

internal sealed class MongoSubscriptionRepository(
    UsersMongoContext context,
    IEnumerable<ISubscriptionPolicy> policies) : IReadWriteSubscriptionRepository
{
    public Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
        => context.GetCollection<SubscriptionDocument>()
            .InsertOneAsync(subscription.ToDocument(), null, cancellationToken);

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
        => context.GetCollection<SubscriptionDocument>()
            .Find(x => x.Type == type)
            .AnyAsync(cancellationToken);

    private async Task<SubscriptionDocument?> GetAsync(Expression<Func<SubscriptionDocument, bool>> expression, CancellationToken cancellationToken = default)
        => await context.GetCollection<SubscriptionDocument>()
            .Find(expression.ToFilterDefinition())
            .SingleOrDefaultAsync(cancellationToken);
}