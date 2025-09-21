using Bogus;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.domain.Subscriptions.Specifications;

namespace discipline.centre.users.tests.shared_kernel.Domain;

public static class SubscriptionFakeDataFactory
{
    public static Subscription GetPremium()
    {
        var faker = new Faker<Subscription>()
            .CustomInstantiator(v =>
                Subscription.Create(
                    SubscriptionId.New(),
                    SubscriptionType.Premium,
                    [
                        new PriceSpecification(
                            v.Random.Decimal(min: 1, max: 30),
                            v.Random.Decimal(min: 100, max: 300),
                            Currency.Pln)
                    ],
                    policies));
        
        return faker.Generate();
    }
    
    public static Subscription GetStandard()
    {
        var faker = new Faker<Subscription>()
            .CustomInstantiator(v =>
                Subscription.Create(
                    SubscriptionId.New(),
                    SubscriptionType.Standard,
                    [],
                    policies));
        
        return faker.Generate();
    }
    
    public static Subscription GetAdmin()
    {
        var faker = new Faker<Subscription>()
            .CustomInstantiator(v =>
                Subscription.Create(
                    SubscriptionId.New(),
                    SubscriptionType.Admin,
                    [],
                    policies));
        
        return faker.Generate();
    }

    private static ISubscriptionPolicy[] policies =
    [
        new AdminSubscriptionPolicy(),
        new PremiumSubscriptionPolicy(),
        new StandardSubscriptionPolicy()
    ];
}