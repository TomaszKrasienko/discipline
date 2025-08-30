using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.users.application.Subscriptions.DTOs;

namespace discipline.centre.users.application.Subscriptions.Queries;

public sealed record GetSubscriptionsQuery : IQuery<IReadOnlyCollection<SubscriptionResponseDto>>;