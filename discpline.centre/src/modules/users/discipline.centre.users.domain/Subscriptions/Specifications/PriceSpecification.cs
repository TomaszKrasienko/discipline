namespace discipline.centre.users.domain.Subscriptions.Specifications;

public sealed record PriceSpecification(
    decimal PerMonth,
    decimal PerYear,
    string Currency);