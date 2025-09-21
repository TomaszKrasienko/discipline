using discipline.centre.users.application.Subscriptions.DTOs;
using discipline.libs.cqrs.abstractions.Queries;

namespace discipline.centre.users.application.Subscriptions.Queries;

public sealed record GetSubscriptionsQuery : IQuery<IReadOnlyCollection<SubscriptionResponseDto>>;