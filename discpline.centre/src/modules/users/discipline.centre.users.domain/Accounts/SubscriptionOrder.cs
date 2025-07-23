using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.Rules.SubscriptionOrders;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts;

public sealed class SubscriptionOrder : Entity<SubscriptionOrderId, Ulid>
{
    public Interval Interval { get; private set; }
    public SubscriptionDetails Subscription { get; }
    public Payment? Payment { get; }
    public SubscriptionId SubscriptionId { get; }

    public bool IsActive => Interval.FinishDate is not null;
    
    /// <summary>
    /// Use only for MongoDB
    /// </summary>
    public SubscriptionOrder(
        SubscriptionOrderId id, 
        Interval interval,
        SubscriptionDetails subscription,
        Payment? payment,
        SubscriptionId subscriptionId) : base(id)
    {
        Interval = interval;
        Subscription = subscription;
        Payment = payment;
        SubscriptionId = subscriptionId;
    }

    public static SubscriptionOrder Create(
        SubscriptionOrderId id,
        Interval interval,
        SubscriptionDetails subscription,
        Payment? payment,
        SubscriptionId subscriptionId)
    {
        CheckRule(new PaymentNotRequireRule(payment, subscription.RequirePayment));
        CheckRule(new PaymentRequireRule(payment, subscription.RequirePayment));
        CheckRule();
        
        return new SubscriptionOrder(
            id,
            interval,
            subscription,
            payment,
            subscriptionId);   
    }

    internal void Finish(TimeProvider timeProvider)
        => Interval = Interval.Create(
            Interval.StartDate,
            Interval.PlannedFinishDate,
            DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime));
}