using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.domain.Subscriptions.ValueObjects;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.domain.Subscriptions;

internal static class SubscriptionDocumentsMapperExtensions
{
    internal static Subscription ToEntity(
        this SubscriptionDocument document,
        ISubscriptionPolicy policy)
        => new (
            SubscriptionId.Parse(document.Id),
            SubscriptionType.FromValue(document.Type),
            document.Prices.Select(x => x.ToEntity()).ToHashSet(),
            policy);

    private static Price ToEntity(this PriceDocument document)
        => new(
            document.PerMonth,
            document.PerYear,
            Currency.FromValue(document.Currency));
}
