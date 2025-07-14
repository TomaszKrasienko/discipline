using Bogus;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;

namespace discipline.centre.users.tests.sharedkernel.Infrastructure;

internal static class AccountDocumentFakeDataFactory
{
    public static AccountDocument Get()
    {
        var faker = new Faker();

        return new AccountDocument()
        {
            Id = Ulid.NewUlid().ToString(),
            Login = faker.Internet.Email(),
            HashedPassword = faker.Random.String(),
            SubscriptionOrders = []
        };
    }

    public static AccountDocument WithSubscriptionOrder(
        this AccountDocument accountDocument,
        bool withPayment = false)
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
                FinishDate = withPayment ? startDate.AddDays(validityPeriod) : null
            },
            SubscriptionDetails = new SubscriptionDetailsDocument
            {
                Type = faker.Random.String(),
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
                    : null 
        };
        
        accountDocument.SubscriptionOrders.Add(subscription);
        
        return accountDocument;
    }
}