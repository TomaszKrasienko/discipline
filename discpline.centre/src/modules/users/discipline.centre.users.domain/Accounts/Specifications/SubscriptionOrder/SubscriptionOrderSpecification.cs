namespace discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;

public sealed record SubscriptionOrderSpecification(
    DateOnly StartDate,
    string SubscriptionType,
    int? ValidityPeriod,
    bool RequirePayment,
    decimal? PaymentValue);