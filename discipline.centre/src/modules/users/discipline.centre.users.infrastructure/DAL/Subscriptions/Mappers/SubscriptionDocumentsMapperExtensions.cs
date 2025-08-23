using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Subscriptions.DTOs;
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

    //TODO: Unit tests
    public static SubscriptionResponseDto ToResponseDto(
        this SubscriptionDocument document,
        IEnumerable<ISubscriptionPolicy> policies)
    {
        var subscriptionType = SubscriptionType.FromValue(document.Type);
        var policy = policies.Single(x => x.CanByApplied(subscriptionType));

        return new SubscriptionResponseDto(
            document.Id,
            document.Type,
            subscriptionType.HasPayment,
            subscriptionType.HasExpiryDate,
            policy.NumberOfDailyTasks(),
            policy.NumberOfRules(),
            document.Prices.Select(x => x.ToResponseDto()).ToList());
    }

    private static SubscriptionPriceResponseDto ToResponseDto(this PriceDocument document)
        => new(
            document.PerMonth,
            document.PerYear,
            document.Currency);
}
