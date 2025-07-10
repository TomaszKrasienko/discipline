namespace discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;

public sealed record SubscriptionOrderSpecification(
    string SubscriptionType,
    int? ValidityPeriod,
    bool RequirePayment,
    decimal? PaymentValue);