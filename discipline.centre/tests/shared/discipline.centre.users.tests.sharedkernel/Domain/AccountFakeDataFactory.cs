using Bogus;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
using discipline.centre.users.domain.Accounts.ValueObjects.Account;
using discipline.centre.users.domain.Subscriptions.Enums;

namespace discipline.centre.users.tests.shared_kernel.Domain;

public static class AccountFakeDataFactory
{
    public static Account Get()
    {
        var faker = new Faker<Account>()
            .CustomInstantiator(v => new Account(
                AccountId.New(),
                v.Internet.Email(),
                Password.Create(v.Random.String(), v.Random.String()),
                []));
        
        return faker.Generate();
    }

    public static Account WithSubscriptionOrder(this Account account,
        bool withPayment = false)
    {
        TimeProvider timeProvider = TimeProvider.System;

        var faker = new Faker();

        var order = new SubscriptionOrderSpecification(
            SubscriptionId.New(),
            faker.Random.String(),
            faker.PickRandom<Period>(Period.GetAvailable()),
            withPayment,
            withPayment ? faker.Random.Decimal(min: 1m) : null);

        account
            .AddOrder(
                SubscriptionOrderId.New(),
                timeProvider,
                order);
        
        return account;
    }
}