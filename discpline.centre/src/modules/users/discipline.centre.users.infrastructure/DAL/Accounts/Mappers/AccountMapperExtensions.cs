using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.domain.Accounts;

//TODO: Unit tests
internal static class AccountMapperExtensions
{
    internal static AccountDocument ToDocument(this Account entity)
        => new()
        {
            Id = entity.Id.ToString(),
            Login = entity.Login.Value,
            HashedPassword = entity.Password.Value,
            SubscriptionOrders = entity.Orders.Select(x => x.ToDocument()).ToList()
        };

    private static SubscriptionOrderDocument ToDocument(this SubscriptionOrder entity)
        => new()
        {
            Id = entity.Id.ToString(),
            Interval = entity.Interval.ToDocument(),
            SubscriptionDetails = entity.Subscription.ToDocument(),
            Payment = entity.Payment?.ToDocument(),
            SubscriptionId = entity.SubscriptionId.ToString()
        };

    private static IntervalDocument ToDocument(this Interval entity)
        => new()
        {
            FinishDate = entity.FinishDate,
            PlanedFinishDate = entity.FinishDate,
            StartDate = entity.StartDate
        };

    private static SubscriptionDetailsDocument ToDocument(this SubscriptionDetails entity)
        => new()
        {
            RequirePayment = entity.RequirePayment,
            Type = entity.Type,
            ValidityPeriod = entity.ValidityPeriod?.Value,
        };

    private static PaymentDocument ToDocument(this Payment entity)
        => new()
        {
            CreatedAt = entity.CreatedAt,
            Value = entity.Value
        };
}