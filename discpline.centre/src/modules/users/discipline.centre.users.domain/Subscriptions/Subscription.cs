using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.domain.Subscriptions.Rules;
using discipline.centre.users.domain.Subscriptions.Specifications;
using discipline.centre.users.domain.Subscriptions.ValueObjects;

namespace discipline.centre.users.domain.Subscriptions;

public sealed class Subscription : AggregateRoot<SubscriptionId, Ulid>
{
    private readonly ISubscriptionPolicy _policy;
    public SubscriptionType Type { get; }
    public Price Price { get; private set; }

    private Subscription(
        SubscriptionId id,
        SubscriptionType type,
        Price price,
        ISubscriptionPolicy policy) : base(id)
    {
        Type = type;
        Price = price;
        _policy = policy;
    }

    public static Subscription Create(
        SubscriptionId id,
        SubscriptionType type,
        PriceSpecification priceSpecification,
        IEnumerable<ISubscriptionPolicy> policies)
    {
        var price = Price.Create(
            priceSpecification.PerMonth,
            priceSpecification.PerMonth,
            priceSpecification.Currency);

        var policy = policies.SingleOrDefault(x => x.CanByApplied(type));

        if (policy is null)
        {
            throw new ArgumentException($"Wrong policy for subscription type: {type.ToString()}");
        }
        
        return new Subscription(id, type, price, policy);
    } 
}