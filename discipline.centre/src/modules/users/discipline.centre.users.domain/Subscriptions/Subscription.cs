using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.domain.Subscriptions.Specifications;
using discipline.centre.users.domain.Subscriptions.ValueObjects;

namespace discipline.centre.users.domain.Subscriptions;

public sealed class Subscription : AggregateRoot<SubscriptionId, Ulid>
{
    // ReSharper disable once MemberInitializerValueIgnored
    private readonly HashSet<Price> _prices = [];
    
    private readonly ISubscriptionPolicy _policy;
    public SubscriptionType Type { get; }
    public IReadOnlySet<Price> Prices => new HashSet<Price>(_prices);

    /// <summary>
    /// Only for mongo purposes
    /// </summary>
    public Subscription(
        SubscriptionId id,
        SubscriptionType type,
        HashSet<Price> prices,
        ISubscriptionPolicy policy) : base(id)
    {
        Type = type;
        _prices = prices;
        _policy = policy;
    }

    public static Subscription Create(
        SubscriptionId id,
        SubscriptionType type,
        HashSet<PriceSpecification> priceSpecifications,
        IEnumerable<ISubscriptionPolicy> policies)
    {
        switch (type.HasPayment)
        {
            case true when !priceSpecifications.Any():
                throw new DomainException("Subscription.RequiredPayment");
            case false when priceSpecifications.Any():
                throw new DomainException("Subscription.PaymentNotRequired");
        }

        var prices = priceSpecifications
            .Select(x => Price.Create(x.PerMonth, x.PerYear, x.Currency))
            .ToHashSet();

        var policy = policies.SingleOrDefault(x => x.CanByApplied(type));

        if (policy is null)
        {
            throw new ArgumentException($"Wrong policy for subscription type: {type.ToString()}");
        }
        
        return new Subscription(id, type, prices, policy);
    }

    public int? GetAllowedNumberOfDailyTasks()
        => _policy.NumberOfDailyTasks();

    public int? GetAllowedNumberOfRules()
        => _policy.NumberOfRules();
}