using discipline.centre.users.domain.Subscriptions.Enums;

namespace discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;

public sealed record SubscriptionOrderSpecification(
    string SubscriptionType,
    Period? Period,
    bool RequirePayment,
    decimal? PaymentValue);