using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions.Enums;

namespace discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;

public sealed record SubscriptionOrderSpecification(
    SubscriptionId SubscriptionId,
    string SubscriptionType,
    Period? Period,
    bool RequirePayment,
    decimal? PaymentValue);