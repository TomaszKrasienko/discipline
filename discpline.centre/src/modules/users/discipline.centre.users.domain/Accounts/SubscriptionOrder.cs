using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts;

public sealed class SubscriptionOrder : Entity<SubscriptionOrderId, Ulid>
{
    public Interval Interval { get; private set; }
    public SubscriptionDetails Subscription { get; private set; }
    public Payment? Payment { get; private set; }
    
    private SubscriptionOrder(
        SubscriptionOrderId id, 
        Interval interval,
        SubscriptionDetails subscription,
        Payment? payment) : base(id)
    {
        Interval = interval;
        Subscription = subscription;
        Payment = payment;
    }

    internal static SubscriptionOrder Create(
        SubscriptionOrderId id, 
        Interval interval,
        SubscriptionDetails subscription, 
        Payment? payment) =>
        new(id, interval, subscription, payment);
}