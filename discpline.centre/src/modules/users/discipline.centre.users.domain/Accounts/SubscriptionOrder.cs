using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.Rules.SubscriptionOrders;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts;

public sealed class SubscriptionOrder : Entity<SubscriptionOrderId, Ulid>
{
    public Interval Interval { get; private set; }
    public SubscriptionDetails Subscription { get; private set; }
    public Payment? Payment { get; private set; }
    
    /// <summary>
    /// Use only for MongoDB
    /// </summary>
    public SubscriptionOrder(
        SubscriptionOrderId id, 
        Interval interval,
        SubscriptionDetails subscription,
        Payment? payment) : base(id)
    {
        Interval = interval;
        Subscription = subscription;
        Payment = payment;
    }

    public static SubscriptionOrder Create(
        SubscriptionOrderId id,
        Interval interval,
        SubscriptionDetails subscription,
        Payment? payment)
    {
        CheckRule(new PaymentNotRequireRule(payment, subscription.RequirePayment));
        CheckRule(new PaymentRequireRule(payment, subscription.RequirePayment));
        
        return new SubscriptionOrder(id, interval, subscription, payment);   
    }
}