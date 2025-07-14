using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.ValueObjects.Account;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;
using discipline.centre.users.domain.Subscriptions.Enums;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.infrastructure.DAL.Accounts.Documents;

internal static class AccountDocumentMapperExtensions
{
    internal static Account ToEntity(this AccountDocument document)
    {
        var id = AccountId.Parse(document.Id);
        var password = new Password(document.HashedPassword);
        
        return new Account(
            id,
            document.Login,
            password,
            document
                .SubscriptionOrders
                .Select(x => x.ToEntity())
                .ToHashSet());
    }

    private static SubscriptionOrder ToEntity(this SubscriptionOrderDocument document)
    {
        var id = SubscriptionOrderId.Parse(document.Id);
        
        var interval = Interval.Create(
            document.Interval.StartDate,
            document.Interval.FinishDate);
        
        var subscriptionDetails = SubscriptionDetails.Create(
            document.SubscriptionDetails.Type,
            document.SubscriptionDetails.ValidityPeriod is null
                ? null
                : Period.FromValue(document.SubscriptionDetails.ValidityPeriod),
            document.SubscriptionDetails.RequirePayment);
        
        Payment? payment = null;

        if (document.Payment is not null)
        {
            payment = new Payment(
                document.Payment.CreatedAt,
                document.Payment.Value);
        }

        return new SubscriptionOrder(
            id,
            interval,
            subscriptionDetails,
            payment);
    }
}