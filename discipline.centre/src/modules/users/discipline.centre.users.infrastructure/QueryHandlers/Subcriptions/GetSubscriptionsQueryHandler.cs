using discipline.centre.users.application.Subscriptions.DTOs;
using discipline.centre.users.application.Subscriptions.Queries;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.infrastructure.DAL;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using discipline.libs.cqrs.abstractions.Queries;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.QueryHandlers.Subcriptions;

internal sealed class GetSubscriptionsQueryHandler(
    UsersMongoContext context,
    IEnumerable<ISubscriptionPolicy> policies) : IQueryHandler<GetSubscriptionsQuery, IReadOnlyCollection<SubscriptionResponseDto>>
{
    public async Task<IReadOnlyCollection<SubscriptionResponseDto>> HandleAsync(GetSubscriptionsQuery query, CancellationToken cancellationToken = default)
    {
        var subscriptionDocuments = await context
            .GetCollection<SubscriptionDocument>()
            .Find(_ => true)
            .ToListAsync(cancellationToken);

        return subscriptionDocuments
            .Select(x => x.ToResponseDto(policies))
            .ToList();
    }
}