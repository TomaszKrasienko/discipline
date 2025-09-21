using Bogus;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;
using Microsoft.AspNetCore.Identity;

namespace discipline.centre.users.tests.shared_kernel.Infrastructure;

internal static class AccountDocumentFakeDataFactory
{
    private static string? _password;
    
    public static AccountDocument Get()
    {
        var faker = new Faker();

        var passwordHasher = new PasswordHasher<Account>();

        _password = "Password123!";
        
        return new AccountDocument()
        {
            Id = Ulid.NewUlid().ToString(),
            Login = faker.Internet.Email(),
            HashedPassword = passwordHasher.HashPassword(null!, _password),
            SubscriptionOrders = []
        };
    }

    public static AccountDocument WithSubscriptionOrder(
        this AccountDocument accountDocument,
        bool withPayment = false,
        SubscriptionId? subscriptionId = null)
    {
        var faker = new Faker();
        var startDate = DateOnly
            .FromDateTime(DateTime.Now.AddDays(faker.Random.Int(min: -30, max: 30)));

        var validityPeriod = faker.Random.Int(min: 1, max: 60);

        var subscription = new SubscriptionOrderDocument
        {
            Id = Ulid.NewUlid().ToString(),
            Interval = new IntervalDocument
            {
                StartDate = startDate,
                PlanedFinishDate = withPayment ? startDate.AddDays(validityPeriod + 1) : null,
                FinishDate = withPayment ? startDate.AddDays(validityPeriod) : null
            },
            SubscriptionDetails = new SubscriptionDetailsDocument
            {
                Type = withPayment ? SubscriptionType.Premium.Value : SubscriptionType.Standard.Value,
                ValidityPeriod = faker.PickRandom<Period>(Period.GetAvailable()).Value,
                RequirePayment = withPayment
            },
            Payment = 
                withPayment 
                    ? new PaymentDocument
                    {
                        Value = faker.Random.Decimal(min:1, max:60),
                        CreatedAt = DateTimeOffset.Now
                    }
                    : null,
            SubscriptionId = subscriptionId is null ? Ulid.NewUlid().ToString() : subscriptionId.Value.ToString(),
        };
        
        accountDocument.SubscriptionOrders.Add(subscription);
        
        return accountDocument;
    }
    
    public static string? GetPassword()
        =>  _password;
}