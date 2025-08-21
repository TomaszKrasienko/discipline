using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.ValueObjects;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

internal static class SubscriptionMapperExtensions
{
    internal static SubscriptionDocument ToDocument(this Subscription entity)
        => new()
        {
            Id = entity.Id.ToString(),
            Type = entity.Type.Value,
            Prices = entity.Prices.Select(x => x.ToDocument()).ToList()
        };

    private static PriceDocument ToDocument(this Price entity)
        => new()
        {
            PerMonth = entity.PerMonth,
            PerYear = entity.PerYear,
            Currency = entity.Currency.Shorcut
        };
}